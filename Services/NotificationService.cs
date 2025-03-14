using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MySqlX.XDevAPI;
using VetManagement.Data;

namespace VetManagement.Services
{
    public class NotificationService
    {

        private readonly Action<string>? _onMessageRecieved;

        public NotificationService(Action<string>? OnMessageRecieved)
        {
            _onMessageRecieved = OnMessageRecieved;
        }

        public async Task StartListening()
        {
            UdpClient client = new UdpClient(9999);
            while (true)
            {
                var result = await client.ReceiveAsync();
                string message = Encoding.UTF8.GetString(result.Buffer);

                _onMessageRecieved?.Invoke(message);
            }
        }

        public static async void SendNotification(string type, string title, string message, string userType)
        {
            UdpClient client = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, new IPEndPoint(IPAddress.Broadcast, 9999));

            Notification notification = new Notification() {Type = type,  Title = title, Message = message, SentAt = DateTime.Now, UserType = userType };
            
            BaseRepository<Notification> notificationRepository = new BaseRepository<Notification>();
            try
            {
                notification = await notificationRepository.Add(notification);

                if (notification == null)
                {
                    Boxes.ErrorBox("Notificarea nu a fost salvată!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Notificarea nu a fost salvată!\n" + e.Message);
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
            Trace.WriteLine(last);
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
