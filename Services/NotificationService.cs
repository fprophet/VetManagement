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
using VetManagement.Data;

namespace VetManagement.Services
{
    public class NotificationService
    {

        private static readonly int Port = 8888;

        private readonly Action<string>? _onMessageRecieved;

        public NotificationService(Action<string>? OnMessageRecieved)
        {
            _onMessageRecieved = OnMessageRecieved;
        }

        public  void StartListening()
        {
            try
            {
                UdpClient client = new UdpClient(Port);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

                while (true)
                {
                    Trace.WriteLine(IPAddress.Any);


                    //var result = await client.ReceiveAsync();

                    byte[] receivedBytes = client.Receive(ref remoteEndPoint);

                    string receivedMessage = Encoding.UTF8.GetString(receivedBytes);
                    //string message = Encoding.UTF8.GetString(result.Buffer);

                    //extract the first 16 bytes represnting the IV
                    byte[] IV = new byte[16];
                    byte[] encryptedMessage = new byte[receivedBytes.Length - 16];

                    Array.Copy(receivedBytes, 0, IV, 0, 16);

                    Array.Copy(receivedBytes, 16, encryptedMessage, 0, receivedBytes.Length - 16);

                    string decrypted = EncryptionService.Decrypt(encryptedMessage, EncryptionService.Key, IV);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _onMessageRecieved?.Invoke(decrypted);
                        // Perform data binding operation here
                    });
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Error receiving message: " + e.Message);
                Logger.LogError("Error", e.ToString());
            }
        }

        public static async void SendNotification(Notification notification)
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

            UdpClient client = new UdpClient();

            int total = client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, Port));

            Boxes.InfoBox("AU fost trimisi:" + total + " biti");

            Trace.WriteLine(IPAddress.Broadcast);
            Trace.WriteLine("UDP signal broadcasted!");


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
