using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class RecipeHistory : BaseEntity
    {
        public int OriginalId { get; set; }
        public int OriginalRegistryNumber { get; set; }
        public int HistoryRegistryNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime? SignedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string MedName { get; set; }
        public bool Signed { get; set; }
        public string OwnerSignature { get; set; }
    }
}
