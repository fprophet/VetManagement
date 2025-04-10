﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Utilities.Encoders;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using VetManagement.Data;
using VetManagement.Services;
using System.Windows.Input;
using System.Windows;
using VetManagement.Commands;
using VetManagement.Stores;
using VetManagement.Repositories;

namespace VetManagement.ViewModels
{
    public class CreateOwnerViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _email = string.Empty;
        private string _address = string.Empty;
        private string _town = string.Empty;
        private string _street = string.Empty;
        private string _streetNumber = string.Empty;
        private string _county = string.Empty;
        private string _phone = string.Empty;
        private string _details = string.Empty;
        private Action<Owner> _onOwnerCreated;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Town
        {
            get => _town;
            set
            {
                _town = value;
                OnPropertyChanged(nameof(Town));
            }
        }
        public string Street
        {
            get => _street;
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }
        
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                _streetNumber = value;
                OnPropertyChanged(nameof(StreetNumber));
            }
        }
        
        public string County
        {
            get => _county;
            set
            {
                _county = value;
                OnPropertyChanged(nameof(County));
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

        public ICommand CreateOwnerCommand { get; set; }

        public CreateOwnerViewModel(NavigationStore navigationStore, Action<Owner> onOwnerCreated)
        {
            _navigationStore = navigationStore;
            _onOwnerCreated = onOwnerCreated;
            CreateOwnerCommand = new RelayCommand(CreateOwner);
        }

        private bool Validate(Owner owner)
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
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
            return true;
        }

        public Owner RetrieveOwner()
        {
            Owner owner = new Owner() 
            {
                Name = Name,
                Town = Town,
                Street = Street,
                StreetNumber = StreetNumber,
                County = County,
                Phone = Phone,
                Details = Details,
                Email = Email,
                DateAdded = DateTime.Now 
            };

            if (!Validate(owner))
            {
                string message = "Completați câmpurile necesare pentru proprietar!\n";

                message += string.Join("\n", Errors.Select(kv => kv.Value.FirstOrDefault())) + "\n";

                Boxes.InfoBox(message);
                Errors.Clear();
                return null;
            }

            return owner;
        }

        private async void CreateOwner(object parameter)
        {
            var owner = RetrieveOwner();

            if (owner == null)
            {
                return;
            }

            try
            {
                BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();
                owner = await ownerRepository.Add(owner);
                _onOwnerCreated?.Invoke(owner);
                var res = Boxes.InfoBox("Pacientul a fost creat!");


                if (res == MessageBoxResult.OK)
                {
                    new CloseWindowCommand<CreateOwnerViewModel>
                        (new WindowService<CreateOwnerViewModel>(_navigationStore, null), this);
                }
            }
            catch (Exception e)
            {

                if (e.InnerException?.Message.ToLower().StartsWith("duplicate entry") == true)
                {
                    Boxes.ErrorBox("Un proprietar cu numele sau numărul de telefon introdus există deja in sistem!");
                }
                else
                {
                    Boxes.ErrorBox("Proprietarul nu a putut fi înregistrat!\n" + e.Message);
                    Logger.LogError("Error", e.ToString());
                }
          
            }
        }
    }
}
