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

            bool settingsFileFreshCreated;

            AppSettings.CheckFile(out settingsFileFreshCreated);
            
            if(settingsFileFreshCreated)
            {
                Boxes.InfoBox("Settings file was not found! It has been freshly created and needs configuration. Root access only!");
                MainWindow = new RootLoginWindow()
                {
                    DataContext = new RootLoginViewModel()
                };

            }else
            {
                if (!SessionManager.Instance.isIsLoggedIn())
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
            }


        
            MainWindow.Show();

        }


    }

}
