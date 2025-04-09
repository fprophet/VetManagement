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

            if(value is null)
            {
                return "";
            }

            TreatmentMed? treatmentMed = value is TreatmentMed ? (TreatmentMed)value : null; 
            TreatmentImportedMed? treatmentImportedMed = value is TreatmentImportedMed ? (TreatmentImportedMed)value : null; 

            if( treatmentMed != null && treatmentMed.Med == null)
            {
                return "";
            }

            if (treatmentImportedMed != null && treatmentImportedMed.ImportedMed == null)
            {
                return "";
            }

            switch ( toDisplay )
            {
                case "Name":
                    if( treatmentMed != null)
                    {
                        return treatmentMed.Med.Name;
                    }
                    return treatmentImportedMed.ImportedMed.Denumire;
                case "Lot":
                    if (treatmentMed != null)
                    {
                        return treatmentMed.Med.LotID;
                    }
                    return "";
                case "Valability":
                    if (treatmentMed != null)
                    {
                        return treatmentMed.Valability.Date.ToString("yyyy-MM-dd");
                    }
                    return "";
                case "Quantity":
                    if (treatmentMed != null)
                    {
                        return treatmentMed.Quantity + " " + treatmentMed.Med.MeasurmentUnit;
                    }
                    return treatmentImportedMed.Quantity;
                case "WaitingTime":
                    if (treatmentMed != null)
                    {
                        return treatmentMed.Med.WaitingTime;
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
