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
    public class Logger
    {
        private static readonly string LogDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetEntryAssembly()?.GetName().Name ?? "VetManagement", "Logs");

        public static async void LogError(string logName, string message)
        {

            string fullPath = LogDirectory + "/" + logName + ".txt";

            Trace.WriteLine("VERIFICAM LOGU");
            Trace.WriteLine(fullPath);
            if (!Directory.Exists(LogDirectory))
            {
                Trace.WriteLine("NARE DIR");

                Directory.CreateDirectory(LogDirectory);
            }
            Trace.WriteLine("AMU ARE DIR");

            if (!File.Exists(fullPath))
            {
                Trace.WriteLine("NUI FISIER");

                File.Create(fullPath).Dispose();
            }
            Trace.WriteLine("AMU ARE FISIER");

            using (StreamWriter writeStream = new StreamWriter(fullPath, true))
            {
                string fullDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                await writeStream.WriteLineAsync("Time:" + fullDateTime);
                await writeStream.WriteLineAsync(message);
            }
        }
    }
}
