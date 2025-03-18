using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class Recipe : BaseEntity
    {
        public DateTime Date { get; set; }

        public DateTime? SignedAt { get; set; }

        public int RegistryNumber { get; set; }

        public string MedName { get; set; }

        public bool Signed { get; set; }

        public string OwnerSignature { get; set; }

        public RegistryRecord RegistryRecord { get; set; }


    }
}
