using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentImportedMed
    {
        public string ImportedMedId { get; set; }

        public int TreatmentId { get; set; }

        [Required]
        public string Quantity { get; set; }

        public string Administration { get; set; }

        public Treatment Treatment { get; set; }

        public ImportedMed ImportedMed { get; set; }
    }
}
