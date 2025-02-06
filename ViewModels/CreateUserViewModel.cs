using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "admin", "user" };

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

        public CreateUserViewModel(Action<User> onUserCreated)
        {
            _onUserCreated = onUserCreated;
            _userRepository = new BaseRepository<User>();
            CreateUserCommand = new RelayCommand(CreateUser);
        }


        private async void CreateUser(object sender)
        {

            if (string.IsNullOrEmpty(Password))
            {
                Trace.WriteLine("Empty pass");
                return;
            }

            User user = new User { Name = Name, Username = Username, Email = Email, Password = PasswordHelper.HashPassword(Password), Role = Role };
            Trace.WriteLine(user);

            try
            {
                await _userRepository.Add(user);
                _onUserCreated?.Invoke(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizatorul nu a putut fi adăugat!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Trace.WriteLine(ex.ToString());
            }

        }

    }
}
