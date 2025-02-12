using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Owner : BaseEntity
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [Required]
        public string Address { get; set; }

        [Required, StringLength(15)]
        public string Phone {  get; set; }

        [EmailAddress]
        [StringLength(200)]
        public string Email {  get; set; }

        public string Details { get; set; }

        public int DateAdded { get; set; }

        public int DateUpdated { get; set; }

        public List<Patient> Patients { get; set; }

        public List<Treatment> Treatments{ get; set; }

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;





    }
}
