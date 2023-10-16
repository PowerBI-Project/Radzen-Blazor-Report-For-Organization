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
    [Route("api/oauth/power-b-i")]
    public class PowerBIController : OAuthAuthenticationCodeControllerBase
    {
        public PowerBIController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(configuration, httpClientFactory)
        {
        }

        public override string ClientId { get => configuration["PowerBI:ClientId"]; }
        public override string ClientSecret { get => configuration["PowerBI:ClientSecret"]; }
        public override string AuthorizationEndpoint { get => configuration["PowerBI:AuthorizationEndpoint"]; }
        public override string TokenEndpoint { get => configuration["PowerBI:TokenEndpoint"]; }
        public override string Scope { get => configuration["PowerBI:Scope"]; }
        public override string RedirectUri { get => $"{Request.Scheme}://{Request.Host}/api/oauth/power-b-i/callback"; }
        public override string Key { get; } = "PowerBI.AuthenticationCode";

        [HttpGet("authorize")]
        public override IActionResult Authorize(string redirectUrl)
        {
            return base.Authorize(redirectUrl);
        }

        [HttpGet("callback")]
        public override async Task<IActionResult> Callback(string code, string error, string state)
        {
            return await base.Callback(code, error, state);
        }

        [HttpPost("token")]
        public override async Task<IActionResult> Token()
        {
            return await base.Token();
        }
    }
}