using System.Configuration;
using System.Data;
using System.Windows;
using esign_app.ViewModels;
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

            AppSettings.CheckFileAndCreate();

            NavigationStore navigationStore = new NavigationStore();

            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

            navigationStore.PageTitle = "🏠 Acasă " + DateTime.Now.ToString("yyyy-MM-dd");

            if (SessionManager.Instance.isIsLoggedIn())
            {
                MainWindow = new UserLogin()
                {
                    DataContext = new UserLoginViewModel(navigationStore)
                };
                navigationStore.Windows[MainWindow] = typeof(MainViewModel).Name;

            }
            else
            {

                MainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel(navigationStore)
                };
                navigationStore.Windows[MainWindow] = typeof(MainViewModel).Name;


            }

            MainWindow.Show();


        }


    }

}
