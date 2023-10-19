using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerBIBlazor.Models.cimut_db
{
    [Table("DataInformasiPenduduks", Schema = "dbo")]
    public partial class DataInformasiPenduduk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NIKNumber { get; set; }

        public string KKNumber { get; set; }

        public string Address { get; set; }

        public int? RTId { get; set; }

        public int? RWId { get; set; }

        public string PostalCode { get; set; }

        public int? KecamatanId { get; set; }

        public int? KelurahanId { get; set; }

        public int? CityId { get; set; }

        public int? ProvinceId { get; set; }

        public int? GenderId { get; set; }

        public string PlaceOfBirth { get; set; }

        public string DateOfBirth { get; set; }

        public int? ReligionId { get; set; }

        public int? EducationId { get; set; }

        public string Occupation { get; set; }

        public int? MaritalStatusId { get; set; }

        public int? RelationshipStatusId { get; set; }

        public string Nationality { get; set; }

        public string PassportNumber { get; set; }

        public string KITASNumber { get; set; }

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public string NIKFilePath { get; set; }

        public string NIKFileName { get; set; }

        public string KKFilePath { get; set; }

        public string KKFileName { get; set; }

        public string CreatedBy { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? CreatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? UpdatedAt { get; set; }

        public string DeletedBy { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? DeletedAt { get; set; }

        public string FullName { get; set; }

        public string KKFileUrl { get; set; }

        public string NIKFileUrl { get; set; }

    }
}