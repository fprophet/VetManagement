using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Invoice : BaseEntity
    {
        [Required]
        public int Number { get; set; }

        [Required]
        public long Date { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public int ProductCount { get; set; }

        public List<Med> Meds { get; set; } 
    }
}
