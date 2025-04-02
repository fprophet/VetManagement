using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    public class CreateRegistryRecordTreatmentViewModel : ViewModelBase
    {

        private readonly int _passedId;

        private string _patientType = "livestock";
        public int PassedId
        {
            get => _passedId;
            set { }
        }

        private readonly FilterService _ownerFilterService, _medFilterService;

        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        public ObservableCollection<Owner> FilteredOwners { get; set; } = new ObservableCollection<Owner>();

        public ObservableCollection<ImportedMed> FilteredImportedMeds { get; set; } = new ObservableCollection<ImportedMed>();

        public ObservableCollection<ImportedMedWrapper> ImportedMedWrappers { get; set; } = new ObservableCollection<ImportedMedWrapper>();


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

        private ImportedMed? _selectedImportedMed;
        public ImportedMed? SelectedImportedMed
        {
            get => _selectedImportedMed;
            set
            {
                _selectedImportedMed = value;
                OnPropertyChanged(nameof(SelectedImportedMed));
                OnMedChanged();
            }
        }


        public ICommand CreateTreatmentCommand { get; }

        public ICommand RemoveMedCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        public ICommand InsertNewMedCommand { get; }

        public ICommand NavigateCreateOwnerWindowCommand { get; }

        public ICommand NavigateCreatePatientWindowCommand { get; }


        private Action<Treatment> _onTreatmentCreateChanged;


        private bool _isMedSearching = false;
        public bool isMedSearching
        {
            get => _isMedSearching;
            set
            {
                _isMedSearching = value;
                OnPropertyChanged(nameof(isMedSearching));
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

        private string _ownerNameSearch;
        public string OwnerNameSearch
        {
            get => _ownerNameSearch;

            set
            {
                _ownerNameSearch = value;
                _ownerFilterService.DebouncePropertyChanged(nameof(OwnerNameSearch));
            }
        }

        private bool _isOwnerListDropdownOpen;
        public bool IsOwnerListDropdownOpen
        {
            get => _isOwnerListDropdownOpen;
            set
            {
                _isOwnerListDropdownOpen = value;
                OnPropertyChanged(nameof(IsOwnerListDropdownOpen));
            }
        }

        private string _medNameSearch;
        public string MedNameSearch
        {
            get => _medNameSearch;

            set
            {
                _medNameSearch = value;
                isMedSearching = true;
                _medFilterService.DebouncePropertyChanged(nameof(MedNameSearch));
            }
        }

        private bool _isMedListDropdownOpen = false;
        public bool IsMedListDropdownOpen
        {
            get => _isMedListDropdownOpen;
            set
            {
                _isMedListDropdownOpen = value;
                OnPropertyChanged(nameof(IsMedListDropdownOpen));
            }
        }

        public CreateRegistryRecordTreatmentViewModel(NavigationStore navigationStore,Action<Treatment> onTreatmentCreateChanged, int? id)
        {
            _navigationStore = navigationStore;
            _onTreatmentCreateChanged = onTreatmentCreateChanged;

            _ownerFilterService = new FilterService(LoadOwners);
            _medFilterService = new FilterService(LoadMeds);

            OnLoadedCommand = new RelayCommand(async (object param) =>
            {
                await LoadOwner();
                await LoadOwnerPatients(_passedId);
            });

            RemoveMedCommand = new RelayCommand(RemoveMed);

            CreateTreatmentCommand = new RelayCommand(CreateTreatmentExecute, CanCreateTreatment);

            if (id.HasValue)
            {
                _passedId = id.Value;
            }
            else
            {
                _passedId = -1; // Example default value
            }



            NavigateCreatePatientWindowCommand = new NavigateWindowCommand<CreatePatientViewModel>
              (new WindowService<CreatePatientViewModel>
                  (new NavigationStore(), (_passedId) => new CreatePatientViewModel(_navigationStore, OnPatientCreated, _passedId)), () => new CreatePatientWindow(), false, false, CanCreatePatient);

            NavigateCreateOwnerWindowCommand = new NavigateWindowCommand<CreateOwnerViewModel>
              (new WindowService<CreateOwnerViewModel>(new NavigationStore(), (_passedId) => new CreateOwnerViewModel(_navigationStore,OnOwnerCreated)), () => new CreateOwnerWindow());

        }

        public bool CanCreatePatient(object parameter)
        {
            return Owner != null;
        }

        private bool CanCreateTreatment(object parameter)
        {
            if (ImportedMedWrappers.Count() < 1)
            {
                return false;
            }

            return Validate(false);

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
            Owner = owner;
        }

        public async void OnOwnerChanged()
        {
            IsOwnerListDropdownOpen = false;

            if( Owner is Owner)
            { 
                await LoadOwnerPatients(Owner.Id);
            }

        }

        public void OnMedChanged()
        {
            IsMedListDropdownOpen = false;

            if (SelectedImportedMed is ImportedMed)
            {
                ImportedMedWrappers.Add(new ImportedMedWrapper(SelectedImportedMed));
                SelectedImportedMed = null;
            }

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


        public async Task LoadOwnerPatients(int? ownerId)
        {
            
            if( ownerId is null)
            {
                return;
            }

            try
            {
                PatientRepository patientRepository = new PatientRepository();
                var patients = await patientRepository.GetForOwnerByType((int)ownerId, _patientType);

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
            if( string.IsNullOrEmpty(MedNameSearch))
            {
                isMedSearching = false;
                return;
            }

            try
            {
                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters.Add("nameFilter", MedNameSearch);

                var (meds,totalPages) = await new ImportedMedRepository().GetAllFiltered(1, -1, filters);

                isMedSearching = false;

                FilteredImportedMeds.Clear();

                foreach (var med in meds)
                {
                    FilteredImportedMeds.Add(med);
                }

                if (FilteredImportedMeds.Count() > 0)
                {
                    IsMedListDropdownOpen = true;
                }
                else
                {
                    IsMedListDropdownOpen = false;
                }
                //MedWrappers.Add(new MedWrapper(MedList[0]));

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare în lista de medicamente\n" + e.Message);
            }
        }

        public async Task LoadOwners()
        {
            if (string.IsNullOrEmpty(OwnerNameSearch))
            {
                return;
            }
            try
            {
                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();

                //var owners = await ownerRepository.GetAll();
                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters.Add("nameFilter", OwnerNameSearch);

                var (owners, totalPages) = await new OwnerRepository().GetFullInfoFiltered(1, -1, filters);

                FilteredOwners.Clear();

                foreach (var owner in owners)
                {
                    FilteredOwners.Add(owner);
                }

                if(FilteredOwners.Count() > 0)
                {
                    IsOwnerListDropdownOpen = true;
                }    
                else
                {
                    IsOwnerListDropdownOpen = false;
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
                var importedMedWrapper = parameter as ImportedMedWrapper;

                if (importedMedWrapper != null)
                {
                    ImportedMedWrappers.Remove(importedMedWrapper);
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Medicamentul nu poate fi șters din listă!\n" + e.Message);
            }
        }

        private bool Validate(bool warnUser = true)
        {
            if (Owner == null)
            {
                if(warnUser)
                {
                    Boxes.ErrorBox("Selectați un proprietar înainte de a creea tratamentul!");
                }
                return false;
            }

            if (Patient == null)
            {
                if (warnUser)
                {
                    Boxes.ErrorBox("Selectați un animal înainte de a creea tratamentul!");
                }
                return false;
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

                var treatment = new Treatment() { PatientId = Patient.Id, OwnerId = Owner.Id, Details = Details, DateAdded = DateTime.Now };

                treatment = await new BaseRepository<Treatment>().Add(treatment);

                if (treatment != null)
                {
                    foreach (var importedMedWrapper in ImportedMedWrappers)
                    {

                        TreatmentImportedMed? treatmentImportedMed = new TreatmentImportedMed()
                        {
                            ImportedMedId = importedMedWrapper.Id,
                            TreatmentId = treatment.Id,
                            Quantity = importedMedWrapper.TreatmentQuantity,
                            Administration = "",
                        };

                        var tmm = await new BaseRepository<TreatmentImportedMed>().Add(treatmentImportedMed);

                        //for display purpose
                        tmm.ImportedMed = importedMedWrapper.ImportedMed;
                        treatment.TreatmentImportedMeds.Add(tmm);
                        treatmentImportedMed = null;
                    }
                    treatment.Patient = Patient;
                    treatment.Owner = Owner;

                    _onTreatmentCreateChanged?.Invoke(treatment);
                    var res = Boxes.InfoBox("Tratamentul a fost adăugat!");

                    if (res == MessageBoxResult.OK)
                    {
                        new CloseWindowCommand<CreateRegistryRecordTreatmentViewModel>
                            (new WindowService<CreateRegistryRecordTreatmentViewModel>(_navigationStore, null), this);
                    }
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
