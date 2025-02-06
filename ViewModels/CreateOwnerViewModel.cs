using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreateOwnerViewModel : ViewModelBase
    {

        private string _name;

        private string _email;

        private string _address;

        private int _phone;

        private string _details;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }


        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public int Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
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

        public CreatePatientViewModel CreatePatientViewModel { get; set; }

        public ICommand CreateOwnerCommand { get; set; }
        public CreateOwnerViewModel(NavigationStore navigationStore)
        {
            CreatePatientViewModel = new CreatePatientViewModel(navigationStore,null);
            _navigationStore = navigationStore;

            CreateOwnerCommand = new RelayCommand(CreateOwner);

        }


        private async void CreateOwner(object parameter)
        {
            try
            {
                Patient patient = CreatePatientViewModel.RetrievePatient();

                Owner owner = new Owner() { Name = Name, Address = Address, Phone = Phone, Details = Details, Email = Email, DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();
                BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();

                owner = await ownerRepository.Add(owner);

                patient.OwnerId = owner.Id;

                await patientRepository.Add(patient);

                Boxes.InfoBox("Pacientul a fost adăugat cu succes!");

            }
            catch (Exception ex)
            {
                Boxes.ErrorBox("Pacientul nu a putut fi înregistrat!\n" + ex.Message);
            }
            //Trace.WriteLine("Pacientul:");
            //Trace.WriteLine(patient.Name);

        }
    }
}
