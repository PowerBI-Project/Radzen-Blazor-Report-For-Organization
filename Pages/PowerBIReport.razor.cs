using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using PowerBIBlazor.Models;

namespace PowerBIBlazor.Pages
{
    public partial class PowerBIReport
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected PowerBIService PowerBIService { get; set; }

        private EmbeddedReportViewModel EmbedToken { get;set; }

        private bool gotReportInfo = false;

        private string errorMessage = String.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                EmbedToken = await PowerBIService.GetEmbedTokenAsync();
                await JSRuntime.InvokeVoidAsync("embedReport",
                    "embed-container",
                    EmbedToken?.Id,
                    EmbedToken?.EmbedUrl,
                    EmbedToken?.Token);

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                gotReportInfo = false;
            }
        }
    }
}