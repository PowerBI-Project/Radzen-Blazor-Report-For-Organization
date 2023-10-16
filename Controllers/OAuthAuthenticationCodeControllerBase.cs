using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using PowerBIBlazor.Models;

namespace PowerBIBlazor.Controllers
{
    public abstract class OAuthAuthenticationCodeControllerBase : Controller
    {
        protected readonly HttpClient httpClient;
        protected readonly IConfiguration configuration;

        protected OAuthAuthenticationCodeControllerBase(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
            this.configuration = configuration;

            if (ClientAuthenticationInHeader)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}")));
            }
        }

        public abstract string ClientId { get; }
        public abstract string AuthorizationEndpoint { get; }
        public abstract string TokenEndpoint { get; }
        public abstract string RedirectUri { get; }
        public abstract string ClientSecret { get; }
        public abstract string Key { get; }
        public abstract string Scope { get; }
        public virtual bool ClientAuthenticationInHeader { get; } = true;

        public virtual IActionResult Authorize(string redirecUrl)
        {
            var url = $"{AuthorizationEndpoint}?response_type=code&scope={Scope}&client_id={ClientId}&redirect_uri={Uri.EscapeDataString(RedirectUri)}&state={Uri.EscapeDataString(redirecUrl)}";

            return Redirect(url);
        }

        private FormUrlEncodedContent GetParameters(IDictionary<string, string> parameters)
        {
            if (!ClientAuthenticationInHeader)
            {
                parameters.Add("client_id", ClientId);
                parameters.Add("client_secret", ClientSecret);
            }

            return new FormUrlEncodedContent(parameters);
        }

        public virtual async Task<IActionResult> Callback(string code, string error, string state)
        {
            var oAuthResponse = string.IsNullOrEmpty(error) ? null : OAuthResponse.Failure(error);

            if (!string.IsNullOrEmpty(code))
            {
                var parameters = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", RedirectUri},
                };

                var response = await httpClient.PostAsync(TokenEndpoint, GetParameters(parameters));

                var json = await response.Content.ReadAsStringAsync();

                oAuthResponse = response.IsSuccessStatusCode ? OAuthResponse.Success(json) : OAuthResponse.Failure(json);
            }

            HttpContext.Session.SetString(Key, JsonSerializer.Serialize(oAuthResponse));

            return Redirect(state);
        }

        public virtual async Task<IActionResult> Token()
        {
            var json = HttpContext.Session.GetString(Key);

            if (json == null)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized);
            }

            var oAuthResponse = JsonSerializer.Deserialize<OAuthResponse>(json);

            var statusCode = HttpStatusCode.OK;

            if (!oAuthResponse.IsSuccess)
            {
                statusCode = HttpStatusCode.Forbidden;
            }
            else if (oAuthResponse.ExpiresAt <= DateTime.UtcNow)
            {
                var parameters = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", oAuthResponse.RefreshToken },
                };

                var response = await httpClient.PostAsync(TokenEndpoint, GetParameters(parameters));

                json = await response.Content.ReadAsStringAsync();

                statusCode = response.StatusCode;

                oAuthResponse = response.IsSuccessStatusCode ? oAuthResponse.Refresh(json) : OAuthResponse.Failure(json);
            }

            return new JsonResult(oAuthResponse)
            {
                StatusCode = (int)statusCode
            };
        }
    }
}