using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace VetManagement.Services
{ 
public class DateToUnixTimestampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long unixTimestamp)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
            }
            if (value is int unixInt)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixInt).DateTime;
            }
            return DateTime.Now;
      
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is DateTime dateTime)
            {
                return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            }
            return 0;

        }

    }
}
