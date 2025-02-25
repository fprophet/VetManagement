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
using System.Diagnostics;
using VetManagement.Stores;
using VetManagement.Commands;
using VetManagement.Views;

namespace VetManagement.ViewModels
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
        public RootLoginViewModel()
        {

            LoginCommand = new RelayCommand(AuthenticateRoot);
        }

        private async void AuthenticateRoot(object parameter)
        {

            try
            {
                Dictionary<string, string> settings = AppSettings.GetSettings();

                bool verifiedPassword = PasswordHelper.VerifyPassword(RootPassword, settings["RootPassword"]);
                bool verifiedUser = PasswordHelper.VerifyPassword(RootUser, settings["RootUser"]);

                if( verifiedPassword && verifiedUser)
                {
                    SessionManager.Instance.LogUser(-1, RootPassword, "admin");

                    NavigationStore navigationStore = new NavigationStore();

                    navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);
                    navigationStore.PageTitle = "Acasă";

                    new NavigateWindowCommand<MainViewModel>
                        (new NavigationService<MainViewModel>(navigationStore, (id) => new MainViewModel(navigationStore)), () => new MainWindow(), true, true);

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
