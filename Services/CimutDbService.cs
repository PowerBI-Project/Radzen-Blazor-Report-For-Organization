using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using PowerBIBlazor.Data;

namespace PowerBIBlazor
{
    public partial class cimut_dbService
    {
        cimut_dbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly cimut_dbContext context;
        private readonly NavigationManager navigationManager;

        public cimut_dbService(cimut_dbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportDataInformasiPenduduksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cimut_db/datainformasipenduduks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cimut_db/datainformasipenduduks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDataInformasiPenduduksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/cimut_db/datainformasipenduduks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/cimut_db/datainformasipenduduks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDataInformasiPenduduksRead(ref IQueryable<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> items);

        public async Task<IQueryable<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk>> GetDataInformasiPenduduks(Query query = null)
        {
            var items = Context.DataInformasiPenduduks.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDataInformasiPenduduksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDataInformasiPendudukGet(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);
        partial void OnGetDataInformasiPendudukById(ref IQueryable<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> items);


        public async Task<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> GetDataInformasiPendudukById(int id)
        {
            var items = Context.DataInformasiPenduduks
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetDataInformasiPendudukById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDataInformasiPendudukGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDataInformasiPendudukCreated(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);
        partial void OnAfterDataInformasiPendudukCreated(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);

        public async Task<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> CreateDataInformasiPenduduk(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk datainformasipenduduk)
        {
            OnDataInformasiPendudukCreated(datainformasipenduduk);

            var existingItem = Context.DataInformasiPenduduks
                              .Where(i => i.Id == datainformasipenduduk.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.DataInformasiPenduduks.Add(datainformasipenduduk);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(datainformasipenduduk).State = EntityState.Detached;
                throw;
            }

            OnAfterDataInformasiPendudukCreated(datainformasipenduduk);

            return datainformasipenduduk;
        }

        public async Task<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> CancelDataInformasiPendudukChanges(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDataInformasiPendudukUpdated(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);
        partial void OnAfterDataInformasiPendudukUpdated(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);

        public async Task<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> UpdateDataInformasiPenduduk(int id, PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk datainformasipenduduk)
        {
            OnDataInformasiPendudukUpdated(datainformasipenduduk);

            var itemToUpdate = Context.DataInformasiPenduduks
                              .Where(i => i.Id == datainformasipenduduk.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(datainformasipenduduk);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDataInformasiPendudukUpdated(datainformasipenduduk);

            return datainformasipenduduk;
        }

        partial void OnDataInformasiPendudukDeleted(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);
        partial void OnAfterDataInformasiPendudukDeleted(PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk item);

        public async Task<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> DeleteDataInformasiPenduduk(int id)
        {
            var itemToDelete = Context.DataInformasiPenduduks
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDataInformasiPendudukDeleted(itemToDelete);


            Context.DataInformasiPenduduks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDataInformasiPendudukDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}