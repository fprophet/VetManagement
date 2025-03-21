using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VetManagement.Data;
using Microsoft.Extensions.Logging;
using VetManagement.Services;
using VetManagement.ViewModels;
using System.Diagnostics;
using VetManagement.Stores;
using VetManagement.Commands;
using VetManagement.Views;
using VetManagement;

namespace esign_app.ViewModels
{
    public class RootLoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; set; }


        private string _username;

        private string _password;

        public string RootUser
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        public string RootPassword
        {
            get => _password;
            set
            {
                _password = value;
            }
        }
        public RootLoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            LoginCommand = new RelayCommand(AuthenticateRoot, (object sender) => !string.IsNullOrEmpty(RootUser) && !string.IsNullOrEmpty(RootPassword));
        }

        private async void AuthenticateRoot(object parameter)
        {

            try
            {

                bool verifiedPassword = PasswordHelper.VerifyPassword(RootPassword, AppSettings.RootPassword);
                bool verifiedUser = PasswordHelper.VerifyPassword(RootUser, AppSettings.RootUser);

                if( verifiedPassword && verifiedUser)
                {
                    SessionManager.Instance.LogUser(-1, RootPassword, "admin");

                    new NavigateWindowCommand<MainViewModel>
                        (new WindowService<MainViewModel>(_navigationStore, (id) => new MainViewModel(_navigationStore)), () => new MainWindow(), true, true);

                }
                else
                {
                    Boxes.ErrorBox("Incorrect username/password!");
                    
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Utilizatorul nu a putut fi verificat!\n" + e.Message);
                Logger.LogError("Database", e.Message);
            }

        }

    }
}
