using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Owner : BaseEntity
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string Name { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Strada este obligatorie!")]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? StreetNumber { get; set; }

        [StringLength(200)]
        [Required(ErrorMessage = "Localitatea este obligatorie!")]
        public string Town { get; set; }

        [StringLength(100)]
        public string? County { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu!"), StringLength(15)]
        public string Phone { get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }

        public string? Details { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }

        public List<Patient> Patients { get; set; }

        public List<Treatment> Treatments { get; set; }

        [NotMapped]
        public string Address
        {
            get => Town + " " + Street + " " + StreetNumber;
            set { }
        }
    }
}
