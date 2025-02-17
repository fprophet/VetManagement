using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentMed : BaseEntity
    {
        public int MedId { get; set; }
        public int TreatmentId { get; set; }

        public float Quantity { get; set; }

        public float Pieces { get; set; }

        public Treatment Treatment { get; set; }

        public Med Med { get; set; }
    }
}
