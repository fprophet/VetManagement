using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VetManagement.Services
{

    public  class AppSettings
    {

        private static readonly string SettingsFile = 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                            "VetManagement", "Settings/Settings.json");

        private static readonly string SettingsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "VetManagement", "Settings");

        private static readonly string _rootUser = "45QIOaMeCRUOuy8GZJNGgRChVyYYzhEQaOL0kg89D3I=";
        private static readonly string _rootPassword = "K5CgXPDn+gfioZs6tNNtz7PmvbWc4zIc12hIwi4WhD8=";

        public static string RootUser
        {
            get => _rootUser;
        }
        public static string RootPassword
        {
            get => _rootPassword;
        }

        public static void CheckFileAndCreate()
        {
            
            if (!Directory.Exists(SettingsDirectory) || !File.Exists(SettingsFile))
            {
                try
                {

                    Directory.CreateDirectory(SettingsDirectory);

                    Dictionary<string, string> settings = new Dictionary<string, string>();

                    settings["Server"] = "";
                    settings["Database"] = "";
                    settings["DatabaseUser"] = "";
                    settings["DatabasePassword"] = "";
                    //settings["RootUser"] = _rootUser;
                    //settings["RootPassword"] = _rootPassword;

                    string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

                    using (StreamWriter writer = new StreamWriter(SettingsFile))
                    {
                        writer.Write(json);
                    }

                }
                catch (Exception e)
                {
                    Boxes.ErrorBox("Could not create settings file!\n" + e.Message);
                }
            }

        }

        public static void SaveSettings(Dictionary<string, string> settings)
        {
            CheckFileAndCreate();

            settings["RootUser"] = _rootUser;
            settings["RootPassword"] = _rootPassword;

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(SettingsFile, json);

        }

        public static Dictionary<string,string> GetSettings()
        {

            if ( !File.Exists(SettingsFile))
            {
                Boxes.ErrorBox("Settings file was not found!\n" + SettingsFile );
                return null;
            }

            string contents = File.ReadAllText(SettingsFile);
            
            Dictionary<string, string> settings = new();
            
            if ( string.IsNullOrEmpty(contents))
            {
                return settings;
            }
            settings = JsonSerializer.Deserialize<Dictionary<string, string>>(contents) ?? new Dictionary<string, string>();

            return settings;
        }


    }
}
