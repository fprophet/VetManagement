using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace VetManagement.Converters
{
    public class AddPixelConverter : IMultiValueConverter
    {
        public double PixelsToAdd { get; set; } = 250;

        private double _scrollbarWidth = 20;

        private double _viewMargins = 40;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)

        {
            if (values[0] is double totalWidth && values[1] is double sideMenuWidth && values[2] is Thickness mainContentMargins)
            {
                return totalWidth - sideMenuWidth - mainContentMargins.Right * 2 - _scrollbarWidth  - _viewMargins;
            }


            return 0;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
