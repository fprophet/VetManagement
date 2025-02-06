using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreatePatientViewModel : ViewModelBase
    {
        private string _name;

        private string _species;

        private string _sex;

        private string _race;

        private int _age;

        private string _color;

        private string _details;

        private readonly int PassedId;

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

        public CreatePatientViewModel( NavigationStore navigationStore, int? id )
        {
            _navigationStore = navigationStore;

            CreatePatientCommand = new RelayCommand(CreatePatient);

            PassedId = (int)id;

        }

        private async void CreatePatient(object parameter)
        {
            if( PassedId <= 0) 
            {
                Boxes.ErrorBox("A fost întampinată o eroare!\n Passed ID is null or 0.");
                return;
            }


            var patient = RetrievePatient();

            patient.OwnerId = PassedId;

            try 
            {
                BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();

                patient = await patientRepository.Add(patient);

                Boxes.InfoBox("Pacientul a fost creat!");
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Pacientul nu a putut fi înregistrat!\n" + e.Message);
            }

        }

        public Patient RetrievePatient()
        {
            return new Patient() { Name = Name, Species = Species, Race = Race, Sex = Sex , Age = Age , Color = Color, Details = Details };
        }
    }
}
