using System;
using System.Globalization;
using System.Windows.Data;
namespace VetManagement.Services
{
    public class CommandParameterToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return value;
            }

            if (value != null && int.TryParse(value.ToString(), out int result))
            {
                return result;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}