using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VetManagement.Commands;
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

        private string _phone;

        private string _details;

        private Action<Owner> _updateOwnersList;
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

        public string Phone
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

        public ICommand NavigateOwnersCommand { get; set; }


        public CreateOwnerViewModel(NavigationStore navigationStore, Action<Owner> updateOwnersList)
        {
            _navigationStore = navigationStore;
            _updateOwnersList = updateOwnersList;

            CreatePatientViewModel = new CreatePatientViewModel(null,null);

            NavigateOwnersCommand = NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            CreateOwnerCommand = new RelayCommand(CreateOwner);

        }

        private bool Validate(Owner owner, Patient patient)
        {
            var validationResults = new List<ValidationResult>();

            var context = new ValidationContext(owner, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(owner, context, validationResults);

            if (!isValid)
            {
                foreach (var error in validationResults)
                {
                    Errors.Add(error.MemberNames.First(), new List<string> { error.ErrorMessage });
                    OnErrorsChanged(error.MemberNames.First());
                }
                return false;
            }

            if (patient == null)
            {
                return false;
            }

            return true;
        }


        private async void CreateOwner(object parameter)
        {
            try
            {
                Patient patient = CreatePatientViewModel.RetrievePatient();

                Owner owner = new Owner() { Name = Name, Address = Address, Phone = Phone, Details = Details, Email = Email, DateAdded = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

                if( !Validate(owner, patient))
                {
                    return;
                }

                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();
                BaseRepository<Patient> patientRepository = new BaseRepository<Patient>();

                owner = await ownerRepository.Add(owner);

                patient.OwnerId = owner.Id;

                await patientRepository.Add(patient);

                _updateOwnersList?.Invoke(owner);
                Boxes.InfoBox("Pacientul a fost adăugat cu succes!");

            }
            catch (Exception ex)
            {
                Boxes.ErrorBox("Pacientul nu a putut fi înregistrat!\n" + ex.Message);
                Logger.LogError("Error", ex.ToString());

            }
            //Trace.WriteLine("Pacientul:");
            //Trace.WriteLine(patient.Name);

        }
    }
}
