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
        private readonly NavigationStore _navigationStore;

        //first item empty to dispaly the first set of inputs
        public ObservableCollection<string> MedList { get; set; } = new ObservableCollection<string>() {""};
        public ObservableCollection<int> QuantityList { get; set; } = new ObservableCollection<int>() { 0 };

        public ObservableCollection<Med> MedsCollection { get; set; } = new ObservableCollection<Med>();

        private int _dateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public ICommand AdMedToListCommand { get; }

        public ICommand CreateTreatmentCommand { get; }

        private string _ownerName;

        private Action<Treatment> _onTreatmnetCreateChanged;
        public string OwnerName
        {
            get => _ownerName;
            set
            {
                _ownerName = value;
                OnPropertyChanged(nameof(OwnerName));
            }
        }

        private string _ownerAddress;
        public string OwnerAddress
        {
            get => _ownerAddress;
            set
            {
                _ownerAddress = value;
                OnPropertyChanged(nameof(OwnerAddress));
            }
        }

        private string _species;
        public string Species
        {
            get => _species;
            set
            {
                _species = value;
                OnPropertyChanged(nameof(Species));
            }
        }

        private string _race;
        public string Race
        {
            get => _race;
            set
            {
                _race = value;
                OnPropertyChanged(nameof(Race));
            }
        }

        private string _sex;
        public string Sex
        {
            get => _sex;
            set
            {
                _sex = value;
                OnPropertyChanged(nameof(Sex));
            }
        }

        private Med _selectedMed;
        public Med SelectedMed
        {
            get => _selectedMed;
            set
            {
                _selectedMed = value;
                OnPropertyChanged(nameof(Med));
            }
        }

        private float _quantity;
        public float Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public int DateAdded
        {
            get => _dateAdded;
            set
            {
                _dateAdded = value;
                OnPropertyChanged(nameof(DateAdded));
            }
        }

        public CreateTreatmentViewModel(Action<Treatment> onTreatmentCreateChanged) 
        {

            _onTreatmnetCreateChanged = onTreatmentCreateChanged;

            AdMedToListCommand = new RelayCommand(AddMedToList);

            CreateTreatmentCommand = new RelayCommand(CreateTreatment);


            LoadMeds();
        }

        private async void LoadMeds()
        {
            try
            {
                BaseRepository<Med> medRepository = new BaseRepository<Med>();

                var meds = await medRepository.GetAll();

                MedsCollection.Clear();

                foreach (var med in meds)
                {
                    MedsCollection.Add(med);
                }

            }catch(Exception e)
            {
                Boxes.ErrorBox("Erare in lista de medicamente\n" + e.Message);
            }

        }

        public void AddMedToList(object parameter)
        {
            MedList.Add("Some dummy string");
        }

        public async void CreateTreatment(object Sender)
        {
            //var obj = { Sender };

            BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();
            BaseRepository<Treatment> treatmentRepository = new BaseRepository<Treatment>();
            BaseRepository<Med> medRepository = new BaseRepository<Med>();


            try
            {

                if (SelectedMed == null || SelectedMed.Quantity < Quantity)
                {
                    Boxes.ErrorBox("Cantitatea de medicament introdusa este mai mică decât cea din stoc!");
                    return;
                }

                var patient = new Patient() { Species = Species, Sex = Sex, Race = Race, DateAdded = DateAdded };
                
                patient = await patientRepository.Add(patient);

                //var treatment = new Treatment() { PatientId = patient.Id, DateUpdated = DateAdded, Quantity = Quantity };

                //treatment = await treatmentRepository.Add(treatment);
                //if (treatment != null)
                //{

                //    SelectedMed.Quantity = SelectedMed.Quantity - Quantity;

                //    await medRepository.Update(SelectedMed);

                //    treatment.Patient = patient;


                //    _onTreatmnetCreateChanged?.Invoke(treatment);
                //    Boxes.InfoBox("Tratamentul a fost adăugat!");
                //}
            }
            catch (Exception e)
            {
                Boxes.ErrorBox(e.ToString());
            }
            //var treatment = new Treatment() { MedId = MedId,  };
        }

    }
}
