using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Med :BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string QuantityType { get; set; }

        [Required]
        public float Quantity { get; set; }

        public string LotID {  get; set; }

        public string Valability { get; set; } 

        public string Description { get; set; }

        public int DateAdded { get; set; }

        public int DateUpdated { get; set; }

        public DateTime DateAddedFormated => DateTimeOffset.FromUnixTimeSeconds(DateAdded).UtcDateTime;

    }
}
