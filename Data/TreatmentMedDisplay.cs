using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public  class TreatmentMedDisplay
    {
        public int TreatmentId { get; set; }

        public bool IsArcihved { get; set; }

        public string Name { get;set; }

        public string Quantity { get; set; }

        public DateTime? Valability { get; set; }

        public string? LotID { get; set; }

        public string? WaitingTime { get; set; }

        public string? Administration { get; set; }


    }
}
