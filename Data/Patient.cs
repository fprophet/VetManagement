using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Patient :BaseEntity
    {

        [Required(ErrorMessage = "Tipul animalului este obligatoriu!"), AllowedValues("pet","livestock")]
        public string Type { get; set; }

        public int? Identifier { get; set; }

        [Required(ErrorMessage = "ID-ul stăpânului este obligatoriu!")]
        public int OwnerId{ get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(20)]
        public string? Color { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Specia animalului este obligatorie!")]
        public string Species { get; set; }

        [Required, AllowedValues("male", "female")]
        public string Sex { get; set; }

        [StringLength(50)]
        [Required (ErrorMessage = "Rasa animalului este obligatorie!")]
        public string Race { get; set; }

        [Required(ErrorMessage = "Vârsta animalului este obligatorie!")]
        public int Age{ get; set; }

        [Required(ErrorMessage = "Greutatea animalului este obligatorie!")]
        public float Weight { get; set; }

        public string? Details { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public Owner Owner { get; set; }
        
        public List<Treatment> Treatments{ get; set; } = new List<Treatment>();



    }
}
