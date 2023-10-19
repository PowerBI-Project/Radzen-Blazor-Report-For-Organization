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
    public partial class DataInformasiPenduduks
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

        protected IEnumerable<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> dataInformasiPenduduks;

        protected RadzenDataGrid<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> grid0;

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            dataInformasiPenduduks = await cimut_dbService.GetDataInformasiPenduduks(new Query { Filter = $@"i => i.NIKNumber.Contains(@0) || i.KKNumber.Contains(@0) || i.Address.Contains(@0) || i.PostalCode.Contains(@0) || i.PlaceOfBirth.Contains(@0) || i.DateOfBirth.Contains(@0) || i.Occupation.Contains(@0) || i.Nationality.Contains(@0) || i.PassportNumber.Contains(@0) || i.KITASNumber.Contains(@0) || i.FatherName.Contains(@0) || i.MotherName.Contains(@0) || i.NIKFilePath.Contains(@0) || i.NIKFileName.Contains(@0) || i.KKFilePath.Contains(@0) || i.KKFileName.Contains(@0) || i.CreatedBy.Contains(@0) || i.UpdatedBy.Contains(@0) || i.DeletedBy.Contains(@0) || i.FullName.Contains(@0) || i.KKFileUrl.Contains(@0) || i.NIKFileUrl.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            dataInformasiPenduduks = await cimut_dbService.GetDataInformasiPenduduks(new Query { Filter = $@"i => i.NIKNumber.Contains(@0) || i.KKNumber.Contains(@0) || i.Address.Contains(@0) || i.PostalCode.Contains(@0) || i.PlaceOfBirth.Contains(@0) || i.DateOfBirth.Contains(@0) || i.Occupation.Contains(@0) || i.Nationality.Contains(@0) || i.PassportNumber.Contains(@0) || i.KITASNumber.Contains(@0) || i.FatherName.Contains(@0) || i.MotherName.Contains(@0) || i.NIKFilePath.Contains(@0) || i.NIKFileName.Contains(@0) || i.KKFilePath.Contains(@0) || i.KKFileName.Contains(@0) || i.CreatedBy.Contains(@0) || i.UpdatedBy.Contains(@0) || i.DeletedBy.Contains(@0) || i.FullName.Contains(@0) || i.KKFileUrl.Contains(@0) || i.NIKFileUrl.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddDataInformasiPenduduk>("Add DataInformasiPenduduk", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> args)
        {
            await DialogService.OpenAsync<EditDataInformasiPenduduk>("Edit DataInformasiPenduduk", new Dictionary<string, object> { {"Id", args.Data.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk dataInformasiPenduduk)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await cimut_dbService.DeleteDataInformasiPenduduk(dataInformasiPenduduk.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete DataInformasiPenduduk"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await cimut_dbService.ExportDataInformasiPenduduksToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "DataInformasiPenduduks");
            }

            if (args == null || args.Value == "xlsx")
            {
                await cimut_dbService.ExportDataInformasiPenduduksToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "DataInformasiPenduduks");
            }
        }
    }
}