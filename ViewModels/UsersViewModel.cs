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
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly UserRepository _userRepository;

        private readonly NavigationStore _navigationStore;

        public ObservableCollection<User> Users { get; private set; }

        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "admin", "user" };

        public ICommand NavigateHomeCommand { get; }

        public ICommand CreateUserCommand { get; }

        public ICommand DeleteUserCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        private string _email;

        private string _name;

        private string _username;

        private string _password;

        private string _role;

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

        public UsersViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _userRepository = new UserRepository();

            LoadUsers();
            Users = new ObservableCollection<User>();
            CreateUserCommand = new RelayCommand(CreateUser);
            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);
            DeleteUserCommand = new RelayCommand(DeleteUser);

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore,() => new HomeViewModel(_navigationStore)));
        }

        private void ToggleFormVisibility(object parameter) {

            isVisibleForm = !isVisibleForm;

        }

        private async void DeleteUser(object parameter) 
        {

            if (parameter is User userToDelete)
            {
                var result = MessageBox.Show($"Sunteti sigur ca doriți să stergeti utilizatorul {userToDelete.Name}?",
                                             "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _userRepository.Delete(userToDelete.Id);
                        Users.Remove(userToDelete);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Utilizatorul nu a putut fi șters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError("Database", e.Message);
                    }

                }
            }
        }

        private async void LoadUsers()
        {

            try
            {
                var users = await _userRepository.GetAll();

                Users.Clear();

                foreach (var user in users)
                {

                    Users.Add(user);
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show("Lista de utilizatori este indisponibilă!\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
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
                Users.Add(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Utilizatorul nu a putut fi adăugat!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Trace.WriteLine(ex.ToString());
            }

        }


    }
}
