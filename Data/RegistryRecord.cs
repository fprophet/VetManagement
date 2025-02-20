using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class RegistryRecord : BaseEntity
    {
        [Required]
        public int TreatmentId { get; set; }

        [Required]
        public long Date { get; set; }

        [Required]
        public string Species { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public float Age { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Symptoms { get; set; }

        [Required]
        public int RecipeNumber { get; set; }

        [Required]
        public int RecipeDate { get; set; }

        [Required]
        public string TreatmentDuration { get; set; }

        [Required, AllowedValues("vindecat","sacrificat","mort")]
        public string Outcome { get; set; }

        [Required]
        public string MedName { get; set; }

        public string Observations { get; set; }

        public Treatment Treatment { get; set; }

        public DateTime DateFormated => DateTimeOffset.FromUnixTimeSeconds(Date).UtcDateTime;

        public string RecipeDateFormated => DateTimeOffset.FromUnixTimeSeconds(Date).UtcDateTime.ToString("yyy-MM-dd");



    }
}
