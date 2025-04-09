using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class RegistryRecordHistory : BaseEntity
    {
        public int OriginalId { get; set; }

        public int OriginalTreatmentId { get; set; }

        public int HistoryTreatmentId { get; set; }

        public int? OriginalRecipeNumber { get; set; }

        public int? HistoryRecipeNumber { get; set; }

        public string TreatmentDuration { get; set; }

        public string Outcome { get; set; }

        public string MedName { get; set; }

        public string Observations { get; set; }
    }
}

