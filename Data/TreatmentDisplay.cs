using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Data
{
    public class TreatmentDisplay
    {
        public int Id { get; set; }
        public bool IsArchived { get; set; }
        public string PatientIdentifier { get; set; }
        public string PatientSex { get; set; }
        public string PatientSpecies { get; set; }
        public string PatientRace { get; set; }
        public int PatientAge { get; set; }
        public double PatientWeight { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddress { get; set; }
        public string Details { get; set; }
        public DateTime DateAdded { get; set; }
        public List<TreatmentMedDisplay> Meds { get; set; }


    }
}
