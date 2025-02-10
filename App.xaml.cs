using System.Configuration;
using System.Data;
using System.Windows;
using VetManagement;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.ViewModels;
using VetManagement.Views;

namespace vet_management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            if (SessionManager.Instance.isIsLoggedIn())
            {
                MainWindow = new UserLogin()
                {
                    DataContext = new UserLoginViewModel()
                };

            }
            else 
            {
                NavigationStore navigationStore = new NavigationStore();

                navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

                navigationStore.PageTitle = "Acasă";

                MainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel(navigationStore)
                };


            }
            MainWindow.Show();

        }


    }

}
