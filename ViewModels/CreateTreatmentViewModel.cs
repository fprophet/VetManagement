using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class CreateTreatmentViewModel : ViewModelBase
    {

        private readonly int _passedId;

        private string _patientType = "pet";
        public int PassedId
        {
            get => _passedId;
            set { }
        }

        public ObservableCollection<Med> MedList { get; set; } = new ObservableCollection<Med>();

        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        public ObservableCollection<Owner> Owners { get; set; } = new ObservableCollection<Owner>();

        public ObservableCollection<MedWrapper> MedWrappers { get; set; } = new ObservableCollection<MedWrapper>();

        private Owner _owner;
        public Owner Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
                OnOwnerChanged();
            }
        }

        public ICommand CreateTreatmentCommand { get; }

        public ICommand RemoveMedCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        public ICommand InsertNewMedCommand { get; }

        public ICommand NavigateCreateOwnerWindowCommand { get; }

        public ICommand NavigateCreatePatientWindowCommand { get; }


        private Action<Treatment> _onTreatmentCreateChanged;


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


        public CreateTreatmentViewModel(Action<Treatment> onTreatmentCreateChanged, int? id, string? patientType)
        {
            _onTreatmentCreateChanged = onTreatmentCreateChanged;

            RemoveMedCommand = new RelayCommand(RemoveMed);

            CreateTreatmentCommand = new RelayCommand(CreateTreatmentExecute);

            InsertNewMedCommand = new RelayCommand(InsertNewMed);

            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);

            if (id.HasValue)
            {
                _passedId = id.Value;
            }
            else
            {
                _passedId = -1; // Example default value
            }

            if( !string.IsNullOrEmpty(patientType))
            {
                _patientType = patientType;
            }


            NavigateCreatePatientWindowCommand = new NavigateWindowCommand<CreatePatientViewModel>
                (new NavigationService<CreatePatientViewModel>(new NavigationStore(), (_passedId) => new CreatePatientViewModel(OnPatientCreated, _passedId)), () => new CreatePatientWindow());
           
            NavigateCreateOwnerWindowCommand = new NavigateWindowCommand<CreateOwnerViewModel>
              (new NavigationService<CreateOwnerViewModel>(new NavigationStore(), (_passedId) => new CreateOwnerViewModel(OnOwnerCreated)), () => new CreateOwnerWindow());

        }
        private void OnPatientCreated(Patient patient)
        {
            Patients.Add(patient);

            var sorted = Patients.OrderByDescending(o => o.DateAdded).ToList();

            Patients.Clear();

            foreach (var sortedPatients in sorted)
            {
                Patients.Add(sortedPatients);
            }

            Patient = patient;

        }

        private void OnOwnerCreated(Owner owner)
        {
            Owners.Add(owner);

            var sorted = Owners.OrderByDescending(o => o.DateAdded).ToList();

            Owners.Clear();

            foreach (var sortedOwner in sorted)
            {
                Owners.Add(sortedOwner);
            }

            Owner = owner;

        }

        public async void OnOwnerChanged()
        {
            await LoadOwnerPatients(Owner.Id);
          
        }

        private void InsertNewMed(Object sender)
        {
            MedWrappers.Add(new MedWrapper(MedList[0]));
        }

        public async Task LoadOwner()
        {
            try
            {
                Owner = await new BaseRepository<Owner>().GetById(_passedId);

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Proprietarul nu a fost făsit!\n" + e.Message);
            }
        }


        public async Task LoadOwnerPatients(int id)
        {
 
            try
            {
                PatientRepository patientRepository = new PatientRepository();
                var patients = await patientRepository.GetForOwnerByType(id, _patientType);

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

                MedList.Clear();

                foreach (var med in meds)
                {
                    MedList.Add(med);
                }
                MedWrappers.Add(new MedWrapper(MedList[0]));

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare în lista de medicamente\n" + e.Message);
            }
        }

        public async Task LoadOwners()
        {
            try
            {
                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();

                var owners = await ownerRepository.GetAll();

                Owners.Clear();

                foreach (var owner in owners)
                {
                    Owners.Add(owner);
                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare în lista de proprietari\n" + e.Message);
            }
        }


        public void RemoveMed(object parameter)
        {
            try
            {
                var medWrapper = parameter as MedWrapper;

                if (medWrapper != null)
                {
                    MedWrappers.Remove(medWrapper);

                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Medicamentul nu poate fi șters din listă!\n" + e.Message);
            }
        }

        private void ToggleFormVisibility(object sender)
        {
            isVisibleForm = !isVisibleForm;
        }

        private bool Validate()
        {
            if (Owner == null)
            {
                Boxes.ErrorBox("Selectați un proprietar înainte de a creea tratamentul!");
                return false;
            }

            if (Patient == null)
            {
                Boxes.ErrorBox("Selectați un animal înainte de a creea tratamentul!");
                return false;
            }

            foreach (var medWrapper in MedWrappers)
            {
                if ((decimal)medWrapper.TotalAmount < medWrapper.TreatmentQuantity)
                {
                    Boxes.ErrorBox("Cantitatea de medicament introdusă pentru " + medWrapper.Name + " este mai mică decât cea din stoc!");
                    return false;
                }

                if( medWrapper.TreatmentQuantity <=0)
                {
                    Boxes.ErrorBox("Cantitatea de medicament introdusă pentru " + medWrapper.Name + " trebuie să fie mai mare decat 0!");
                    return false;
                }
            }

            return true;
        }

        private async void CreateTreatmentExecute(object sender)
        {
            await CreateTreatment();
        }

        public async Task<bool> CreateTreatment()
        {
           
            if(!Validate())
            {
                return false;
            }

            try
            {
                BaseRepository<TreatmentMed> tretmentMedRepository = new BaseRepository<TreatmentMed>();
                BaseRepository<Treatment> treatmentRepository = new BaseRepository<Treatment>();
                BaseRepository<Med> medRepository = new BaseRepository<Med>();

                var treatment = new Treatment() { PatientId = Patient.Id, OwnerId = Owner.Id, Details = Details, DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                treatment = await treatmentRepository.Add(treatment);
                if (treatment != null)
                {
                    foreach (var medWrapper in MedWrappers)
                    {
                        medWrapper.TotalAmount = medWrapper.TotalAmount - medWrapper.TreatmentQuantity;

                        //update the medWrapper.Med directly to avoid triggering propchange event
                        medWrapper.Med.Pieces = decimal.Round(medWrapper.TotalAmount / medWrapper.PerPiece, 1);

                        await medRepository.Update(medWrapper.Med);

                        var tm = await tretmentMedRepository.Add(new TreatmentMed()
                        {
                            MedId = medWrapper.Id,
                            TreatmentId = treatment.Id,
                            Quantity = medWrapper.TreatmentQuantity,
                            Pieces = 1,//decimal.Round(medWrapper.TreatmentQuantity / medWrapper.PerPiece,2),
                            Administration = "",
                        });
                        Trace.WriteLine(medWrapper.TotalAmount);

                        //for display purpose
                        tm.Med = medWrapper.Med;
                        treatment.TreatmentMeds.Add(tm);
                    }
                    treatment.Patient = Patient;
                    treatment.Owner = Owner;

                    _onTreatmentCreateChanged?.Invoke(treatment);
                    Boxes.InfoBox("Tratamentul a fost adăugat!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox(e.ToString());
                return false;
            }
            return true;
        }

    }
}
