using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace PowerBIBlazor.Pages
{
    public partial class EditDataInformasiPenduduk
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
        public cimut_dbService cimut_dbService { get; set; }

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            dataInformasiPenduduk = await cimut_dbService.GetDataInformasiPendudukById(Id);
        }
        protected bool errorVisible;
        protected PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk dataInformasiPenduduk;

        protected async Task FormSubmit()
        {
            try
            {
                await cimut_dbService.UpdateDataInformasiPenduduk(Id, dataInformasiPenduduk);
                DialogService.Close(dataInformasiPenduduk);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           cimut_dbService.Reset();
            hasChanges = false;
            canEdit = true;

            dataInformasiPenduduk = await cimut_dbService.GetDataInformasiPendudukById(Id);
        }
    }
}