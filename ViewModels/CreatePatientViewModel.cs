using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreatePatientViewModel : ViewModelBase
    {
        private string _name;

        private string _type = "pet";

        private int? _identifier;

        private string _species;

        private string _sex;

        private string _race;

        private int _age;

        private float _weight;

        private string _color = String.Empty;

        private string _details;

        private readonly int PassedId;

        private Action<Patient> _onPatientCreated;

        public ObservableCollection<object> TypeList { get; set; } = 
            new ObservableCollection<object>() { new { Name = "Animal de companie", Value = "pet" }, new { Name = "Animal mare", Value = "livestock" } };

        private List<string> _customErrors = new();

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnErrorsChanged(nameof(Type));
            }
        }

        public int? Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                OnPropertyChanged(nameof(Identifier));
            }
        }

        public string Name
        {
            get => _name;
            set 
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(Species));
            }
        }

        public string Race
        {
            get => _race;
            set
            {
                _race = value;
                OnPropertyChanged(nameof(Race));
            }
        }

        public string Sex
        {
            get => _sex;
            set
            {
                _sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }   
        
        public float Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public string Details
        {
            get => _details;
            set
            {
                _details = value;
                OnPropertyChanged(nameof(Details));
            }
        }

        public ICommand CreatePatientCommand { get; set; }

        public ICommand NavigateOwnersCommand { get; set; }

        public CreatePatientViewModel(NavigationStore navigationStore,Action<Patient>? OnPatientCreated, int? id)
        {
            _navigationStore = navigationStore;
            _onPatientCreated = OnPatientCreated;

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            CreatePatientCommand = new RelayCommand(CreatePatient);

              if (id.HasValue)
            {
                PassedId = id.Value;
            }
            else
            {
                PassedId = -1;
            }
        }

        private async void CreatePatient(object parameter)
        {
            if( PassedId <= 0) 
            {
                Boxes.ErrorBox("A fost întampinată o eroare!\n Passed ID is null or 0.");
                return;
            }

            var patient = RetrievePatient();

            if( patient == null )
            {
                return;
            }

            patient.OwnerId = PassedId;

            try 
            {
                BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();

                patient = await patientRepository.Add(patient);

                _onPatientCreated?.Invoke(patient);

                var res = Boxes.InfoBox("Pacientul a fost creat!");


                if (res == MessageBoxResult.OK)
                {
                    new CloseWindowCommand<CreatePatientViewModel>
                        (new WindowService<CreatePatientViewModel>(_navigationStore, null), this);
                }
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Pacientul nu a putut fi înregistrat!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }

        }

        private bool CustomValidation(Patient patient)
        {
            if(string.IsNullOrEmpty(patient.Name) && patient.Identifier == null)
            {
                _customErrors.Add("Trebuie sa adăugați Numele sau Numărul de identificare!");
            }

            if (patient.Age <= 0)
            {
                _customErrors.Add("Vârsta pacientului trebuie sa fie mai mare decat 0!");
            }

            if (patient.Weight <= 0)
            {
                _customErrors.Add("Greutatea pacientului trebuie sa fie mai mare decat 0!");
            }

            return _customErrors.Count() == 0;
        }

        private bool Validate( Patient patient)
        {

            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(patient, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(patient, context, validationResults);

            bool customValidation = CustomValidation(patient);

            if (!isValid || !customValidation)
            {
                foreach (var error in validationResults)
                {
                    Errors.Add(error.MemberNames.First(), new List<string> { error.ErrorMessage });
                    OnErrorsChanged(error.MemberNames.First());
                }
                return false;
            }

            return true;
        }

        public Patient? RetrievePatient()
        {
            Patient patient =  new Patient() {Type = Type, Identifier = Identifier, Name = Name, Species = Species, Race = Race, Sex = Sex , Age = Age, Weight = Weight, Color = Color, Details = Details , DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

            if (!Validate(patient))
            {
                string message = "Completați câmpurile necesare pentru animal!\n";

                message += string.Join("\n", Errors.Select(kv => kv.Value.FirstOrDefault())) + "\n";
                message += string.Join("\n", _customErrors);

                Boxes.InfoBox(message);
                Errors.Clear();
                _customErrors.Clear();
                return null;
            }

            return patient;
        }
    }
}
