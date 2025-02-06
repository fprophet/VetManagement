using System;
using System.Globalization;
using System.Windows.Data;
namespace VetManagement.Services
{
    public class CommandParameterToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is already an int
            if (value is int)
            {
                return value;
            }

            // If it's not an int, attempt to convert it (or return a default value, e.g., 0)
            if (value != null && int.TryParse(value.ToString(), out int result))
            {
                return result;
            }

            return 0; // Return a default value (could be another fallback value)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // In most cases, we don't need to implement ConvertBack for CommandParameter
            return Binding.DoNothing;
        }
    }
}