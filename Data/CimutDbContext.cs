using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PowerBIBlazor.Models.cimut_db;

namespace PowerBIBlazor.Data
{
    public partial class cimut_dbContext : DbContext
    {
        public cimut_dbContext()
        {
        }

        public cimut_dbContext(DbContextOptions<cimut_dbContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk>()
              .Property(p => p.CreatedAt)
              .HasColumnType("datetime2");

            builder.Entity<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk>()
              .Property(p => p.UpdatedAt)
              .HasColumnType("datetime2");

            builder.Entity<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk>()
              .Property(p => p.DeletedAt)
              .HasColumnType("datetime2");
            this.OnModelBuilding(builder);
        }

        public DbSet<PowerBIBlazor.Models.cimut_db.DataInformasiPenduduk> DataInformasiPenduduks { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}