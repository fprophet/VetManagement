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
        public DateTime Date { get; set; }

        public int? RecipeNumber { get; set; }

        [Required(ErrorMessage = "Câmpul 'Durata Tratamentului' este obligatoriu!")]
        public string TreatmentDuration { get; set; }

        [Required(ErrorMessage = "Câmpul 'Rezultat' este obligatoriu!"), AllowedValues("vindecat","sacrificat","mort")]
        public string Outcome { get; set; }

        [Required(ErrorMessage = "Câmpul 'Numele Medicului' este obligatoriu!")]
        public string MedName { get; set; }

        public string Observations { get; set; }

        public Treatment Treatment { get; set; }

        public Recipe Recipe { get; set; }

        //public DateTime DateFormated => DateTimeOffset.FromUnixTimeSeconds(Date).UtcDateTime;

        //public string RecipeDateFormated => DateTimeOffset.FromUnixTimeSeconds(Date).UtcDateTime.ToString("yyy-MM-dd");



    }
}
