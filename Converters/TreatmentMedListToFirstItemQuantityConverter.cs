﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VetManagement.Data;

namespace VetManagement.Converters
{
    public class TreatmentMedListToFirstItemQuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<TreatmentMed> tmList && tmList.Count > 0)
            {
                Trace.WriteLine(tmList[0]);
                return tmList[0].Quantity;
            }

            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
