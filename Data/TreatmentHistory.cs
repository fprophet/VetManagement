using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentHistory : BaseEntity
    {

        public int OriginalId { get; set; }
        //patient columns
        [Required]
        public int PatientId { get; set; }

        [Required, StringLength(10)]
        public string PatientType { get; set; }

        [Required, StringLength(200)]
        public string PatientIdentifier { get; set; }

        [Required, StringLength(100)]
        public string PatientSpecies { get; set; }

        [Required, StringLength(100)]
        public string PatientRace { get; set; }

        [Required, StringLength(100)]
        public string PatientSex { get; set; }

        [Required]
        public int PatientAge { get; set; }

        [Required]
        public float PatientWeight { get; set; }

        //owner columns
        [Required]
        public int OwnerId { get; set; }

        [Required, StringLength(200)]
        public string OwnerName { get; set; }

        [Required, StringLength(400)]
        public string OwnerAddress { get; set; }

        //self columns
        public DateTime DateAdded { get; set; }

        public DateTime DateDeleted { get; set; }

        public string? Details { get; set; }

        public List<TreatmentMedHistory> TreatmentMedHistory { get; set; } = new List<TreatmentMedHistory>();
    }
}
