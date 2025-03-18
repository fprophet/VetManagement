using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VetManagement.Services
{
    public class MedQuantityToBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Count() < 2 || values[0] == null || values[1] == null)
            {
                return System.Windows.Media.Brushes.White;
            }

            decimal requiredQuantity = System.Convert.ToDecimal(values[0]);
            decimal totalQuantity = System.Convert.ToDecimal(values[1]);
            SolidColorBrush red = (SolidColorBrush)new BrushConverter().ConvertFromString("#f15f5f");

            if (requiredQuantity > totalQuantity)
            {
                return red;
            }

            return System.Windows.Media.Brushes.White;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
