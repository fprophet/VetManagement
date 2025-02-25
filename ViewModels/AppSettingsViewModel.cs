using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class AppSettingsViewModel : ViewModelBase
    {

        private string _server;
        public string Server
        {
            get => _server;
            set
            {
                _server = value;
                OnPropertyChanged(nameof(Server));
            }
        }

        private string _database;
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged(nameof(Database));
            }
        }

        private string _databaseUser;
        public string DatabaseUser
        {
            get => _databaseUser;
            set
            {
                _databaseUser = value;
                OnPropertyChanged(nameof(DatabaseUser));
            }
        }

        private string _databasePassword;
        public string DatabasePassword
        {
            get => _databasePassword;
            set
            {
                _databasePassword = value;
                OnPropertyChanged(nameof(DatabasePassword));
            }
        }

        public ICommand SaveSettingsCommand { get; set; }


        public AppSettingsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            GetSettings();

            SaveSettingsCommand = new RelayCommand(SaveSettings);

        }

        private void SaveSettings(object sender)
        {
            try
            {
                Dictionary<string, string> settings = new();

                settings["Server"] = Server;
                settings["Database"] = Database;
                settings["DatabaseUser"] = DatabaseUser;
                settings["DatabasePassword"] = DatabasePassword;

                AppSettings.SaveSettings(settings);

                Boxes.InfoBox("Settings saved!");
            }catch(Exception e)
            {

                Boxes.ErrorBox("Settings could not be saved!\n" + e.Message);
            }

        }

        private void GetSettings()
        {
            try
            {
                Dictionary<string,string> settings = AppSettings.GetSettings();
                if ( settings.Count == 0)
                {
                    return;
                }

                Server = settings["Server"] ?? "";
                Database = settings["Database"] ?? "";
                DatabaseUser = settings["DatabaseUser"] ?? "";
                DatabasePassword = settings["DatabasePassword"] ?? "";

            }
            catch(Exception e)
            {
                Boxes.ErrorBox(e.Message);
            }
        }

    }
}
