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
        [Key]
        public int Id { get; set; }

        [Required]
        public int OwnerId{ get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [StringLength(50)]
        [Required]
        public string Species { get; set; }

        [Required, AllowedValues("male", "female")]
        public string Sex { get; set; }

        [StringLength(50)]
        [Required]
        public string Race { get; set; }

        public int Age{ get; set; }

        public string Details { get; set; }

        public int DateAdded { get; set; }

        public int DateUpdated { get; set; }

        public Owner Owner { get; set; }
        public List<Treatment> Treatments{ get; set; } = new List<Treatment>();



    }
}
