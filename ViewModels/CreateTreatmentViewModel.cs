using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreateTreatmentViewModel : ViewModelBase
    {
        private readonly int PassedId;

        public ObservableCollection<Med> Meds { get; set; } = new ObservableCollection<Med>();

        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        public ObservableCollection<MedInputPair> MedInputPair { get; set; } = new ObservableCollection<MedInputPair>();

        public ICommand CreateTreatmentCommand { get; }

        public ICommand AddInputPairCommand { get; }

        public ICommand RemoveInputPairCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }


        private Action<Treatment> _onTreatmnetCreateChanged;


        private bool _isVisibleForm = false;
        public bool isVisibleForm
        {
            get => _isVisibleForm;
            set
            {
                _isVisibleForm = value;
                OnPropertyChanged(nameof(isVisibleForm));
            }
        }

        private Patient _patient;
        public Patient Patient
        {
            get => _patient;
            set
            {
                _patient = value;
                OnPropertyChanged(nameof(Patient));
            }
        }

        private string _details;
        public string Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged(nameof(Details));
            }
        }


        public CreateTreatmentViewModel(Action<Treatment> onTreatmentCreateChanged, int? id) 
        {
            _onTreatmnetCreateChanged = onTreatmentCreateChanged;

            AddInputPairCommand = new RelayCommand(AddInputPair);

            RemoveInputPairCommand = new RelayCommand(RemoveInputPair);

            CreateTreatmentCommand = new RelayCommand(CreateTreatment);

            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);


            if (id.HasValue)
            {
                PassedId = id.Value;
            }
            else
            {
                PassedId = -1; // Example default value
            }

        }

        public async Task LoadOwnerPatients()
        {

            if( PassedId <= 0)
            {
                Boxes.ErrorBox("Lista de animale nu a putut fi redată!\n Id not found!");
                return;
            }

            try
            {
                PatientRepository patientRepository = new PatientRepository();
                var patients = await patientRepository.GetForOwner((int)PassedId);

                Patients.Clear();
                foreach (var patient in patients)
                {
                    Patients.Add(patient);
                }
            }
            catch (Exception ex)
            {
                Boxes.ErrorBox("Lista de animale nu a putut fi redată!\n" + ex.Message);
            }
        }

        public async Task LoadMeds()
        {
            try
            {
                BaseRepository<Med> medRepository = new BaseRepository<Med>();

                var meds = await medRepository.GetAll();

                Meds.Clear();
                
                foreach (var med in meds)
                {
                    Meds.Add(med);
                }

                MedInputPair.Add(new Data.MedInputPair { Med = Meds[0], Quantity = 0, Rank = 0 });

                //Trace.WriteLine("AICI");
                //Trace.WriteLine(Meds.Count);
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Eroare în lista de medicamente\n" + e.Message);
            }
        }

        public void AddInputPair(object parameter)
        {
            int count = MedInputPair.Count();
            MedInputPair.Add(new MedInputPair { Med = Meds[0], Quantity = 0, Rank = count });
        }

        public void RemoveInputPair(object parameter)
        { 
            try 
            {
                MedInputPair.Remove(MedInputPair[(int)parameter]);

                for ( int i = 0; i < MedInputPair.Count(); i++)
                {
                    MedInputPair[i].Rank = i;
                }
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Medicamentul nu poate fi șters din listă!\n" + e.Message);
            }

        }

        private void ToggleFormVisibility(object sender)
        {
            isVisibleForm = !isVisibleForm;
        }

        public async void CreateTreatment(object Sender)
        {

            BaseRepository<TreatmentMed> tretmentMedRepository = new BaseRepository<TreatmentMed>();
            BaseRepository<Treatment> treatmentRepository = new BaseRepository<Treatment>();
            BaseRepository<Med> medRepository = new BaseRepository<Med>();

            if( Patient == null)
            {
                Boxes.ErrorBox("Selectați un animal înainte de a creea tratamentul!");
                return;
            }

            try
            {

                foreach( var pair in MedInputPair)
                {
                    //Trace.WriteLine("Medicament: " + pair.Med.Name);
                    //Trace.WriteLine("Cantitate: " + pair.Quantity);
                    //Trace.WriteLine("Rank: " + pair.Rank);
                    if ((float)pair.Med.TotalAmount < pair.Quantity)
                    {
                        Boxes.ErrorBox("Cantitatea de medicament introdusă pentru " + pair.Med.Name + " este mai mică decât cea din stoc!");
                        return;
                    }

                }
              
                var treatment = new Treatment() { PatientId = Patient.Id, OwnerId = PassedId, Details = Details, DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                treatment = await treatmentRepository.Add(treatment);
                if (treatment != null)
                {
                    //loop again in order to avoid meds quantity update if creating a treatment object fails
                    foreach (var pair in MedInputPair)
                    {
                        pair.Med.TotalAmount = pair.Med.TotalAmount - pair.Quantity;
                        pair.Med.Pieces = pair.Med.TotalAmount/pair.Med.PerPiece;


                        await medRepository.Update(pair.Med);

                        var tm = await tretmentMedRepository.Add(new TreatmentMed() { MedId = pair.Med.Id, TreatmentId = treatment.Id, Quantity = pair.Quantity, Pieces = pair.Quantity / pair.Med.PerPiece  });

                        //for display purpose
                        tm.Med = pair.Med;
                        treatment.TreatmentMeds.Add(tm);
                    }
                    treatment.Patient = Patient;


                    _onTreatmnetCreateChanged?.Invoke(treatment);
                    Boxes.InfoBox("Tratamentul a fost adăugat!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox(e.ToString());
            }
            //var treatment = new Treatment() { MedId = MedId,  };
        }

    }
}
