using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class OwnerHistory : BaseEntity
    {
        public int OriginalId { get; set; }

        [Required,StringLength(100)]
        public string Name { get; set; }

        [Required,StringLength(200)]
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

        [StringLength(200)]
        public string? Email { get; set; }

        public string? Details { get; set; }

        public DateTime DateDeleted { get; set; }

        public DateTime DateAdded { get; set; }


    }
}
