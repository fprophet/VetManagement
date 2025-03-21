using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }
        public ICommand LoginCommand { get; set; }
       

        private bool _loginAsRoot = false;

        public bool LoginAsRoot
        {
            get => _loginAsRoot;
            set
            {
                _loginAsRoot = value;
                OnPropertyChanged(nameof(LoginAsRoot));
            }
        }


        private string _username;

        private string _password;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public UserLoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            LoginCommand = new RelayCommand(LogUser, CanExecute);
        
        }


        private async void LogUser(object parameter)
        {

            if (_loginAsRoot)
            {
                AuthenticateRoot();

            }
            else 
            {
                AuthenticateUser();
            }

        }

        private void AuthenticateRoot()
        {
            try
            {

                bool verifiedPassword = PasswordHelper.VerifyPassword(Password, AppSettings.RootPassword);
                bool verifiedUser = PasswordHelper.VerifyPassword(Username, AppSettings.RootUser);

                if (verifiedPassword && verifiedUser)
                {
                    SessionManager.Instance.LogUser(-1, Password, "admin");

                    new NavigateWindowCommand<MainViewModel>
                        (new WindowService<MainViewModel>(_navigationStore, (id) => new MainViewModel(_navigationStore)), () => new MainWindow(), true, true);

                }
                else
                {
                    Boxes.Warning("Incorrect username/password!");

                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Utilizatorul nu a putut fi verificat!\n" + e.Message);
                Logger.LogError("Database", e.Message);
            }
        }

        private async void AuthenticateUser()
        {
            UserRepository userRepository = new UserRepository();
            User user = null;
            try
            {
                user = await userRepository.GetByUsername(Username);
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare la conectare baza de date!");
                Logger.LogError("Database", e.Message);
                return;
            }
            if (user != null)
            {
                if (PasswordHelper.VerifyPassword(Password, user.Password))
                {
                    SessionManager.Instance.LogUser(user.Id, user.Username, user.Role);
                    new NavigateWindowCommand<MainViewModel>
                        (new WindowService<MainViewModel>(_navigationStore, (id) => new MainViewModel(_navigationStore)), () => new MainWindow(), true, true);
                }
                else
                {
                    Boxes.Warning("Nume sau parola greșită!");
                    return;
                }
            }
            else
            {
                Boxes.Warning("Nume sau parola greșită!");
                return;
            }
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

    }
}
