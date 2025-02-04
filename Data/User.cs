using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, AllowedValues("admin", "user")]
        public string Role { get; set; }


    }
}
