using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class CreateUserViewModel : ViewModelBase
    {
        private readonly IRepository<User> _userRepository;

        private readonly Action<User> _onUserCreated;

        // Add a property for Username
        private string _email;

        private string _name;

        private string _username;

        private string _password;

        private string _role;

        //to display in the dropdown meny
        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "manager", "farmacist","asistent" };

        public SecureString SecurePassword { private get; set; }

        public ICommand CreateUserCommand { get; }
        public ICommand LoginCommand { get; }


        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public CreateUserViewModel(NavigationStore navigationStore,Action<User> onUserCreated)
        {
            _navigationStore = navigationStore;
            _onUserCreated = onUserCreated;
            _userRepository = new BaseRepository<User>();
            CreateUserCommand = new RelayCommand(CreateUser);
        }


        private bool Validate(User user)
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            var context = new ValidationContext(user, serviceProvider: null, items: null);

            bool isValid = Validator.TryValidateObject(user, context, validationResults);

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

        private async void CreateUser(object sender)
        {
            User user = new User { Name = Name, Username = Username, Email = Email, Password = Password, Role = Role };
            if (!Validate(user))
            {
                return;
            }

            user.Password = PasswordHelper.HashPassword(Password);
       
            try
            {
                await _userRepository.Add(user);
                _onUserCreated?.Invoke(user);
                var res = Boxes.InfoBox("Utilizatorul a fost creat!");
                if (res == MessageBoxResult.OK)
                {
                    new CloseWindowCommand<CreateUserViewModel>
                        (new WindowService<CreateUserViewModel>(_navigationStore, null), this);
                }

            }
            catch (Exception ex)
            {
                Boxes.ErrorBox("Utilizatorul nu a putut fi adăugat!\n" + ex.Message);

            }

        }

    }
}
