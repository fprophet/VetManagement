using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MySqlX.XDevAPI;
using VetManagement.Services;

namespace VetManagement.Converters
{
    public class UserRoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Trace.WriteLine(SessionManager.Instance.Role);
            if( SessionManager.Instance.Role != "admin" && SessionManager.Instance.Role != "manager" && SessionManager.Instance.Role != "farmacist")
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
