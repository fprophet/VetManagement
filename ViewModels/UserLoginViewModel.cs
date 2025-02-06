using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class UserLoginViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel { get; }
        public ICommand LoginCommand { get; set; }

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
        public UserLoginViewModel()
        {

            LoginCommand = new RelayCommand(AuthenticateUser, CanExecute);
        }

        private async void AuthenticateUser(object parameter)
        {
            BaseRepository<User> userRepository = new BaseRepository<User>();

            try
            {
                //var user = await userRepository.GetByUsername(Username);
                //if (user != null)
                //{
                //    Trace.WriteLine("HERE");

                //    if (PasswordHelper.VerifyPassword(Password, user.Password))
                //    {
                //        SessionManager.Instance.LogUser(user.Id, user.Username, user.Role);

                //        //var mainMenu = new HomeView();


                //        //Application.Current.MainWindow.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Nume sau parola greșite!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //    }

                //}
            }catch (Exception e)
            {
                MessageBox.Show("Utilizatorul nu a putut fi verificat!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                Logger.LogError("Database", e.Message);
            }

        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

    }
}
