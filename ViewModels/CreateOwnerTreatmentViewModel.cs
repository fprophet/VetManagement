using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    public class CreateOwnerTreatmentViewModel : ViewModelBase
    {
        private readonly int _passedId;

        public int PassedId
        {
            get => _passedId;
            set { }
        }

        public ObservableCollection<Med> MedList { get; set; } = new ObservableCollection<Med>();

        public ObservableCollection<Patient> Patients { get; set; } = new ObservableCollection<Patient>();

        public ObservableCollection<MedWrapper> MedWrappers { get; set; } = new ObservableCollection<MedWrapper>();

        private Owner _owner ;
        public Owner Owner 
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
         
        public ICommand CreateTreatmentCommand { get; }

        public ICommand RemoveMedCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        public ICommand InsertNewMedCommand { get; }

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


        public CreateOwnerTreatmentViewModel(NavigationStore navigationStore, Action<Treatment> onTreatmentCreateChanged, int? id) 
        {
            _navigationStore = navigationStore;
            _onTreatmentCreateChanged = onTreatmentCreateChanged;

            RemoveMedCommand = new RelayCommand(RemoveMed);

            CreateTreatmentCommand = new RelayCommand(CreateTreatment);

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


            NavigateCreatePatientWindowCommand = new NavigateWindowCommand<CreatePatientViewModel>
                (new WindowService<CreatePatientViewModel>(new NavigationStore(), (_passedId) => new CreatePatientViewModel(_navigationStore,OnPatientCreated, _passedId)), () => new CreatePatientWindow());

          

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
            catch(Exception e)
            {
                Boxes.ErrorBox("Proprietarul nu a fost făsit!\n" + e.Message);
            }
        }


        public async Task LoadOwnerPatients()
        {

            if( _passedId <= 0)
            {
                Boxes.ErrorBox("Lista de animale nu a putut fi redată!\n Id not found!");
                return;
            }

            try
            {
                PatientRepository patientRepository = new PatientRepository();
                var patients = await patientRepository.GetForOwner((int)_passedId);

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

                //foreach (var medwrapepr in MedList)
                //{
                //    Trace.WriteLine(medwrapepr.Name);
                //    Trace.WriteLine(medwrapepr.PerPiece);
                //    Trace.WriteLine("--------------------");

                //}


            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare în lista de medicamente\n" + e.Message);
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

                foreach( var medWrapper in MedWrappers)
                {
                    if ((decimal)medWrapper.TotalAmount < medWrapper.TreatmentQuantity)
                    {
                        Boxes.ErrorBox("Cantitatea de medicament introdusă pentru " + medWrapper.Name + " este mai mică decât cea din stoc!");
                        return;
                    }

                }
              
                var treatment = new Treatment() { PatientId = Patient.Id, OwnerId = _passedId, Details = Details, DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                treatment = await treatmentRepository.Add(treatment);
                if (treatment != null)
                {
                    //loop again in order to avoid meds quantity update if creating a treatment object fails
                    foreach (var medWrapper in MedWrappers)
                    {

                        medWrapper.TotalAmount = medWrapper.TotalAmount - medWrapper.TreatmentQuantity;

                        //update the Med directly to avoid triggering propchange event
                        medWrapper.Med.Pieces = (int) Math.Ceiling(medWrapper.TotalAmount / medWrapper.PerPiece);

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


                    _onTreatmentCreateChanged?.Invoke(treatment);
                    var res = Boxes.InfoBox("Tratamentul a fost adăugat!");


                    if (res == MessageBoxResult.OK)
                    {
                        new CloseWindowCommand<CreateOwnerTreatmentViewModel>
                            (new WindowService<CreateOwnerTreatmentViewModel>(_navigationStore, null), this);
                    }
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
