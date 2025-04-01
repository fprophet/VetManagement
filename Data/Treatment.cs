using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Treatment :BaseEntity
    {
        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int OwnerId { get; set; }

        public Owner Owner{ get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public string? Details { get; set; }

        public List<TreatmentMed> TreatmentMeds{ get; set; } = new List<TreatmentMed>();
        public List<TreatmentImportedMed> TreatmentImportedMeds { get; set; } = new List<TreatmentImportedMed>();

    }
}
