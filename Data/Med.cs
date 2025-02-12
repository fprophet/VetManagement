using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Med :BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string QuantityType { get; set; }

        [Required]
        public float Quantity { get; set; }

        public string LotID {  get; set; }

        public long Valability { get; set; } 

        public string Description { get; set; }

        public long DateAdded { get; set; }

        public long DateUpdated { get; set; }

        public List<TreatmentMed> TreatmentMeds { get; set; } = new List<TreatmentMed>();

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;

        public DateTime ValabilityFormated => DateTimeOffset.FromUnixTimeSeconds(Valability).UtcDateTime;

    }
}
