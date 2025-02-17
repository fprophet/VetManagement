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
        [Key]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; }

        public int OwnerId { get; set; }

        public Owner Owner{ get; set; }

        public int DateAdded { get; set; }

        public int DateUpdated { get; set; }

        public string? Details { get; set; }

        public List<TreatmentMed> TreatmentMeds{ get; set; } = new List<TreatmentMed>();

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;
    }
}
