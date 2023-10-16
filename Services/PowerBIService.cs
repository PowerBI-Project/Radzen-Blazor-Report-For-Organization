using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

using PowerBIBlazor.Models;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
namespace PowerBIBlazor
{
    public partial class PowerBIService
    {
        [Inject]
        protected IConfiguration Configuration {get; set;}


        private readonly HttpClient httpClient;

        private OAuthResponse oauthResponse;
        private readonly NavigationManager navigationManager;
        private readonly HttpClient server;

        public async Task<OAuthResponse> GetToken()
        {
            if (oauthResponse != null && oauthResponse.ExpiresAt > DateTime.UtcNow)
            {
                return oauthResponse;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "api/oauth/power-b-i/token");

            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await server.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                navigationManager.NavigateTo($"api/oauth/power-b-i/authorize?redirectUrl={navigationManager.Uri}", true);
            }
            else if (response.IsSuccessStatusCode)
            {
                oauthResponse = await response.Content.ReadFromJsonAsync<OAuthResponse>();

                if (!oauthResponse.IsSuccess)
                {
                    throw new Exception($"Unable to get token: {oauthResponse.Error}");
                }
            }

            return oauthResponse;
        }

        private readonly string clientId ;
        private readonly string clientSecret ;
        private readonly string workspaceId ;
        private readonly string reportId;
        private readonly string powerBiApiUrl;

        public PowerBIService(NavigationManager navigationManager, IHttpClientFactory httpClientFactory,  IConfiguration configuration)
        {
            this.httpClient = httpClientFactory.CreateClient("PowerBI");
            this.navigationManager = navigationManager;
            this.server = httpClientFactory.CreateClient("PowerBIBlazor");
            this.server.BaseAddress = new Uri(navigationManager.BaseUri);
            
             // Ambil nilai dari appsettings.json
            clientId = configuration["PowerBI:ClientId"];
            clientSecret = configuration["PowerBI:ClientSecret"];
            workspaceId = configuration["Report:WorkspaceId"];
            reportId = configuration["Report:ReportId"];
            powerBiApiUrl = configuration["PowerBI:Url"];
        }

        private async Task AuthorizeRequest(HttpRequestMessage request)
        {
            var token = await GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }


        
        public async Task<EmbeddedReportViewModel> GetEmbedTokenAsync()
        {
            var token = await GetToken();

            using (var client = new PowerBIClient(new Uri(powerBiApiUrl), new TokenCredentials(token?.AccessToken, "Bearer")))
            {
                Report report;

                // Settings' workspace ID is not empty
                if (!string.IsNullOrEmpty(workspaceId))
                {
                    // Gets a report from the workspace.
                    report = GetReportFromWorkspace(client, workspaceId, reportId);
                }
                // Settings' report and workspace Ids are empty, retrieves the user's first report.
                else if (string.IsNullOrEmpty(reportId))
                {
                    report = client.Reports.GetReports().Value.FirstOrDefault();
                    AppendErrorIfReportNull(report, "No reports found. Please specify the target report ID and workspace in the applications settings.");
                }
                // Settings contains report ID. (no workspace ID)
                else
                {
                    var reportGuId = new Guid(reportId);
                    report = client.Reports.GetReports().Value.FirstOrDefault(r => r.Id == reportGuId);
                    AppendErrorIfReportNull(report, $"Report with ID: '{reportId}' not found. Please check the report ID. For reports within a workspace with a workspace ID, add the workspace ID to the application's settings");
                }
                var reportViewModel = new EmbeddedReportViewModel(
                    Id : report?.Id.ToString(),
                    Name : report?.Name,
                    EmbedUrl : report?.EmbedUrl,
                    Token : oauthResponse?.AccessToken
                );
                
                return reportViewModel;
            }
        }

        private Report GetReportFromWorkspace(PowerBIClient client, string WorkspaceId, string reportId)
        {
            // Gets the workspace by WorkspaceId.
            var workspaces = client.Groups.GetGroups();
            var workspaceGuId = new Guid(WorkspaceId);
            var reportGuId = new Guid(reportId);
            var sourceWorkspace = workspaces.Value.FirstOrDefault(g => g.Id == workspaceGuId);

            // No workspace with the workspace ID was found.
            //if (sourceWorkspace == null)
           // {
             //   errorLabel.Text = $"Workspace with id: '{WorkspaceId}' not found. Please validate the provided workspace ID.";
               // return null;
            //}

            Report report = null;
            if (string.IsNullOrEmpty(reportId))
            {
                // Get the first report in the workspace.
                report = client.Reports.GetReportsInGroup(sourceWorkspace.Id).Value.FirstOrDefault();
                AppendErrorIfReportNull(report, "Workspace doesn't contain any reports.");
            }

            else
            {
                try
                {
                    // retrieve a report by the workspace ID and report ID.
                    report = client.Reports.GetReportInGroup(workspaceGuId, reportGuId);
                }

                catch(HttpOperationException)
                {
                   

                }
            }

            return report;
        }

        private void AppendErrorIfReportNull(Report report, string errorMessage)
        {
            if (report == null)
            {
                
            }
        }
    }
}