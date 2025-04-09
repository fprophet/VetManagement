using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VetManagement.Converters
{
    public class BooleanToImportingStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isImporting)
            {
                if(parameter is string param)
                {
                    if (param == "start")
                    {
                        return isImporting ? Visibility.Collapsed : Visibility.Visible; 
                    }
                    else if (param == "importing")
                    {
                        return isImporting ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
