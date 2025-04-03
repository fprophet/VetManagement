using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Diagnostics;

namespace VetManagement.Converters
{
    public class ViewTemplateConverter : IValueConverter
    {
        public Dictionary<string, DataTemplate> TemplateMap { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string key && TemplateMap != null && TemplateMap.ContainsKey(key))
            {
                return TemplateMap[key]; // Return matching template
            }

            return null; // Return null to hide the ContentControl
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
