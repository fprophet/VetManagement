using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentMedHistory : BaseEntity
    {
        public int TreatmentHistoryId { get; set; }

        public string OriginalMedId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Quantity { get; set; }

        public string WaitingTime { get; set; }

        public string Administration { get; set; }

        public int InvoiceNumber { get; set; }

        public string LotID { get; set; }

        public DateTime? Valability { get; set; }

        public TreatmentHistory TreatmentHistory { get; set; }

    }
}
