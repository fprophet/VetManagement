using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VetManagement.Services
{
    class Logger
    {
        private static readonly string LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetEntryAssembly().GetName().Name, "Logs");
        
        public static async void LogError(string logName, string message) {

            string fullPath = LogDirectory  + "/" + logName + ".txt";

            if (!Directory.Exists(LogDirectory)) 
            {
                Directory.CreateDirectory(LogDirectory);
            }

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath);
            }

            using (StreamWriter writeStream = new StreamWriter(fullPath, true))
            {
                string fullDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                writeStream.WriteLine("Time:" + fullDateTime);
                writeStream.WriteLine( message);

            }

        }
    }
}
