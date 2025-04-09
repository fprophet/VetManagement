using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;

namespace VetManagement.DataWrappers
{
    public class TreatmentWrapper : BaseWrapper
    {
        private Treatment _treatment;

        public Treatment Treatment
        {
            get => _treatment;
            set
            {
                _treatment = value;
                OnPropertyChanged(nameof(Treatment));
            }
        }  
        
        public Owner Owner
        {
            get => _treatment.Owner;
            set
            {
                _treatment.Owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }   
        
        public Patient Patient
        {
            get => _treatment.Patient;
            set
            {
                _treatment.Patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        public TreatmentWrapper(Treatment treatment)
        {
            Treatment = treatment;
        }

        public int PatientId
        {
            get => _treatment.PatientId;
            set
            {
                _treatment.PatientId = value;
                OnPropertyChanged(nameof(PatientId));
            }
        }

        public int OwnerId
        {
            get => _treatment.OwnerId;
            set
            {
                _treatment.OwnerId = value;
                OnPropertyChanged(nameof(OwnerId));
            }
        }


        public DateTime DateAdded
        {
            get => _treatment.DateAdded;
            set
            {
                _treatment.DateAdded = value;
                OnPropertyChanged(nameof(DateAdded));

            }
        }

        public DateTime DateUpdated
        {
            get => _treatment.DateUpdated;
            set
            {
                _treatment.DateUpdated = value;
                OnPropertyChanged(nameof(DateUpdated));

            }
        }

        public string OwnerAddress
        {
            get => _treatment.OwnerAddress;
            set
            {
                _treatment.OwnerAddress = value;
                OnPropertyChanged(nameof(OwnerAddress));

            }
        }
        
        public double PatientWeight
        {
            get => _treatment.PatientWeight;
            set
            {
                _treatment.PatientWeight = value;
                OnPropertyChanged(nameof(PatientWeight));

            }
        }      

        public int PatientAge
        {
            get => _treatment.PatientAge;
            set
            {
                _treatment.PatientAge = value;
                OnPropertyChanged(nameof(PatientAge));

            }
        }
        
        public string Details
        {
            get => _treatment.Details;
            set
            {
                _treatment.Details = value;
                OnPropertyChanged(nameof(Details));

            }
        }

        public List<TreatmentMed> TreatmentMeds
        {
            get => _treatment.TreatmentMeds;
            set
            {
                _treatment.TreatmentMeds = value;
                OnPropertyChanged(nameof(TreatmentMeds));

            }
        }

        public List<TreatmentImportedMed> TreatmentImportedMeds
        {
            get => _treatment.TreatmentImportedMeds;
            set
            {
                _treatment.TreatmentImportedMeds = value;
                OnPropertyChanged(nameof(TreatmentImportedMeds));

            }
        }

        public object Meds
        {
            get
            {
                if (TreatmentMeds.Count() == 0)
                {
                    return TreatmentImportedMeds;
                }
                return TreatmentMeds;
            }

            set { }
        }


    }

}
