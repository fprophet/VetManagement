using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public  class TreatmentsViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        private BaseRepository<Treatment> _treatmentRepository;

        private BaseRepository<Patient> _patientRepository;

        private BaseRepository<Med> _medRepository;

        private CreateTreatmentViewModel _CreateTreatmentViewModel;

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();

        public TreatmentsViewModel( NavigationStore navigationStore) 
        { 
            _navigationStore = navigationStore;
            _CreateTreatmentViewModel = new CreateTreatmentViewModel(OnTreatmentCreated);
            _treatmentRepository = new BaseRepository<Treatment>();   
            LoadTreatments();
        }

        private void OnTreatmentCreated( Treatment treatment)
        {
            Treatments.Add(treatment);
        }


        private async void LoadTreatments()
        {
            //var treatments = await _treatmentRepository.GetFullTreatments();

            //Trace.WriteLine("AICI");

            //foreach(var treatment in treatments)
            //{
            //    Trace.WriteLine(treatment.Patient.OwnerName);

            //    Treatments.Add(treatment);
            //}

        }
    }
}
