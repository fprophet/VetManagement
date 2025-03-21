using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VetManagement.Services
{
    public class Boxes
    {
        public static void ErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ConfirmBox(string message)
        {
            return MessageBox.Show(message, "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        }

        public static MessageBoxResult InfoBox( string message)
        {
           return MessageBox.Show(message,"Info",MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult Warning(string message)
        {
            return MessageBox.Show(message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
