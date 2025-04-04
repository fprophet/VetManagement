using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VetManagement.Data;

namespace VetManagement.Converters
{
    public class MedOrImportedMedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            string toDisplay = (string)parameter;

            if ( string.IsNullOrEmpty(toDisplay) )
            {
                return "";
            }



            switch( toDisplay )
            {
                case "Name":
                    if( value is TreatmentMed)
                    {
                        return ((TreatmentMed)value).Med.WaitingTime;
                    }
                    return ((TreatmentImportedMed)value).ImportedMed.Denumire;
                case "Lot":
                    if (value is TreatmentMed)
                    {
                        return ((TreatmentMed)value).Med.WaitingTime;
                    }
                    return "";
                case "Valability":
                    if (value is TreatmentMed)
                    {
                        return ((TreatmentMed)value).Med.Valability.Date.ToString("yyyy-MM-dd");
                    }
                    return "";
                case "Quantity":
                    if (value is TreatmentMed)
                    {
                        return ((TreatmentMed)value).Quantity;
                    }
                    return ((TreatmentImportedMed)value).Quantity;
                case "WaitingTime":
                    if (value is TreatmentMed)
                    {
                        return ((TreatmentMed)value).Med.WaitingTime;
                    }
                    return "";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;  // No need for reverse conversion
        }
    }
}
