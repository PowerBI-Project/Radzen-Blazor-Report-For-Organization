using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using PowerBIBlazor.Data;

namespace PowerBIBlazor.Controllers
{
    public partial class Exportcimut_dbController : ExportController
    {
        private readonly cimut_dbContext context;
        private readonly cimut_dbService service;

        public Exportcimut_dbController(cimut_dbContext context, cimut_dbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/cimut_db/datainformasipenduduks/csv")]
        [HttpGet("/export/cimut_db/datainformasipenduduks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDataInformasiPenduduksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDataInformasiPenduduks(), Request.Query), fileName);
        }

        [HttpGet("/export/cimut_db/datainformasipenduduks/excel")]
        [HttpGet("/export/cimut_db/datainformasipenduduks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDataInformasiPenduduksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDataInformasiPenduduks(), Request.Query), fileName);
        }
    }
}
