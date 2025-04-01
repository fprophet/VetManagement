using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using MySqlX.XDevAPI;
using NPOI.SS.Formula.Functions;
using VetManagement.Data;

namespace VetManagement.Services
{
    public class NotificationService
    {

        public static void SendNotification(Notification notification)
        {
          
            if (!string.IsNullOrEmpty(notification.Type) && notification.Type != "recipe-signed" && notification.Type != "to-sign")
            {
                CreateNotification(notification);
            }

            string json = JsonSerializer.Serialize(notification, new JsonSerializerOptions { WriteIndented = true });

            //create the IV for encryption
            string iv = EncryptionService.GenerateKey(16);
            byte[] IV = Encoding.UTF8.GetBytes(iv);

            //encrypt the message
            byte[] encrypted = EncryptionService.Encrypt(json, EncryptionService.Key, IV);

            //create the data array iv + message
            byte[] data = new byte[encrypted.Length + IV.Length];


            Array.Copy(IV, 0, data,0, IV.Length);

            Array.Copy(encrypted, 0, data,IV.Length, encrypted.Length);


            TcpConnection.Instance.SendMessage(Encoding.UTF8.GetBytes(json));
            //TcpConnection.Instance.SendMessage(data);


        }

        private static async void CreateNotification(Notification notification)
        {
            try
            {
                notification = await new BaseRepository<Notification>().Add(notification);

                if (notification == null)
                {
                    Boxes.ErrorBox("Notificarea nu a fost salvată!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Notificarea nu a fost salvată!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
        }

        public static async Task<Notification> GetLastNotification()
        {
            return await new BaseRepository<Notification>().LastRecord();
        }

        public static int RecipeIdFromTitle(string title)
        {
            string[] parts = title.Split(":");

            string last = parts.Last();
            if (int.TryParse(parts.Last(), out int id))
            {
                return id;
            }
            else
            {
                Trace.WriteLine("Failed to parse Recipe ID from title.");
                return -1;
            }
        }
    }
}
