using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MathNet.Numerics;
using Mysqlx;
using NPOI.POIFS.Crypt;
using NPOI.Util;
using SixLabors.ImageSharp.Memory;
using static NPOI.HSSF.Util.HSSFColor;

namespace VetManagement.Services
{
    public class TcpConnection
    {
        private TcpClient? client;
        private NetworkStream stream;
        private const string ServerPassword = "Sperla12345";

        private static TcpConnection _instance;
        public static TcpConnection Instance => _instance ?? (_instance = new TcpConnection());

        private Action<string>? _onMessageRecieved;

        public void SetRecieveCallBack(Action<string> callback)
        {
            _onMessageRecieved = callback;
        }


        public async Task ConnectToServer()
        {
            try
            {
                client = new TcpClient("192.168.100.197", 5000); // Change to your Linux server IP
                stream = client.GetStream();

                if (AuthenticateWithServer())
                {
                    _ = Task.Run(() => ListenForMessages());
                }

            }
            catch (Exception ex)
            {
                Boxes.ErrorBox($"Connection error: {ex.Message}");
            }
        }

        private bool AuthenticateWithServer()
        {
            byte[] buffer = new byte[1024];

            //read from server
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (serverResponse.Contains("Enter password:"))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(ServerPassword + "\n");
                stream.Write(passwordBytes, 0, passwordBytes.Length);
            }

            //read again
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            serverResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);


            if (serverResponse.Contains("Authentication failed"))
            {
                Boxes.ErrorBox("❌ Authentication failed! Disconnecting...");
                client?.Close();
                return false;
            }

            return true;
        }

        private async void ListenForMessages()
        {
            while (true)
            {

                byte[] lengthBuffer = new byte[4];
                int bytesRead = 0;

                // Read message length
                while (bytesRead < 4)
                {
                    int read = await stream.ReadAsync(lengthBuffer, bytesRead, 4 - bytesRead);
                    if (read == 0)
                    {
                        Trace.WriteLine("Client disconnected.");
                        return;
                    }
                    bytesRead += read;
                }

                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                if (messageLength <= 0 || messageLength > 30000)
                {
                    continue;
                }

                byte[] buffer = new byte[messageLength];
                int totalRead = 0;

                Trace.WriteLine("lungime: " + messageLength);
                // Read full message
                while (totalRead < messageLength)
                {
                    int read = await stream.ReadAsync(buffer, totalRead, messageLength - totalRead);
                    if (read == 0)
                    {
                        return;
                    }
                    totalRead += read;
                }

                // Verify complete read
                if (totalRead == messageLength)
                {
                    HandleRecievedMessage(buffer);
                }

                else
                {
                    return;
                }

                byte[] leftoverBuffer = new byte[2048];
                int leftover = await stream.ReadAsync(leftoverBuffer, 0, leftoverBuffer.Length);

                //Trace.WriteLine("Waiting for next message...");
            }
        }


        private void HandleRecievedMessage(byte[] buffer)
        {


            byte[] IV = new byte[16];

            byte[] messageBuffer = new byte[buffer.Length - 16];

            Array.Copy(buffer, 0, IV, 0, 16);

            Array.Copy(buffer, 16, messageBuffer, 0, buffer.Length - 16);
            string encrypted = Encoding.UTF8.GetString(buffer);

            //string encrypted = EncryptionService.Decrypt(messageBuffer, EncryptionService.Key, IV);

            Trace.WriteLine(encrypted);
            Application.Current.Dispatcher.Invoke(() =>
            {
                _onMessageRecieved?.Invoke(encrypted);
            });
        }

        private byte[] PrepareForBroadcast(byte[] buffer)
        {
            int totalBytes = buffer.Length;

            byte[] lengthBuffer = new byte[4];

            lengthBuffer = BitConverter.GetBytes(totalBytes);
            byte[] data = new byte[lengthBuffer.Length + buffer.Length];

            Buffer.BlockCopy(lengthBuffer, 0, data, 0, lengthBuffer.Length);
            Buffer.BlockCopy(buffer, 0, data, lengthBuffer.Length, buffer.Length);

            return data;
        }
            
        public async void SendMessage(byte[] message)
        {
            try
            {
                byte[] data = PrepareForBroadcast(message);

                await stream.WriteAsync(data, 0, data.Length);
                Boxes.InfoBox("Sent: " + message.Length + "bytes"!);
            }
            catch (Exception ex)
            {
                Boxes.ErrorBox($"Error sending message: {ex.Message}");
            }
        }
    }
}
