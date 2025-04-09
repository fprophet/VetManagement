using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class User :BaseEntity
    {

        [StringLength(50)]
        [Required]
        public string Username { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, AllowedValues("admin","manager","user"), StringLength(10)]
        public string Role { get; set; }
        



    }
}
