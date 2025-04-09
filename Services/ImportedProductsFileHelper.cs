using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.Util.Collections;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class ImportedMedsFileHelper
    {

        public Action<int,int,string> OnProgressChanged;

        public ImportedMedsFileHelper(Action<int, int, string> onProgressChanged)
        {
            OnProgressChanged = onProgressChanged;
        }

        public async Task<bool> ImportMeds(string path, CancellationToken cancellationToken)
        {
            //path = "C:\\Users\\eduar\\Desktop\\produse.xls";

            if (!File.Exists(path))
            {
                Boxes.InfoBox("Fișierul nu a fost găsit!");
                return false;
            }

            List<string> importedMedProperties = ObjectHelper.GetPropertiesAsStrings(typeof(ImportedMed));

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);

                var firstRow = sheet.GetRow(0);

                var collCount = firstRow.LastCellNum;

                if (collCount != importedMedProperties.Count() -1)
                {
                    Boxes.InfoBox("Fișierul nu coincide cu șablonul de bază!");
                    Logger.LogError("Error", "IMPORTED MEDS ERROR: Column count not matching!\n" +
                        "File:" + collCount + "\n" +
                        "Database: " + importedMedProperties.Count());
                    return false;
                }

                for (int i = 0; i < collCount; i++)
                {
                    var cell = firstRow.GetCell(i);

                    if (cell.ToString() != importedMedProperties[i])
                    {
                        Boxes.InfoBox("Fișierul nu coincide cu șablonul de bază!");
                        Logger.LogError("Error", "IMPORTED PRODUCTS ERROR: Column count not matching!\n" +
                           "File column:" + cell.ToString() + "\n" +
                           "Database column: " + importedMedProperties[i]);
                        return false;
                    }
                }

                for (int i = 1; i < sheet.LastRowNum; i++)
                {
                    if(cancellationToken.IsCancellationRequested)
                    {
                        return false;   
                    }

                    var row = sheet.GetRow(i);
                    var importedMed = new ImportedMed();

                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        var propertyName = importedMedProperties[j];
                        var propertyValue = row.GetCell(j).ToString();
                        var property = typeof(ImportedMed).GetProperty(propertyName);

                        if( string.IsNullOrEmpty(propertyValue) && property != null)
                        {
                            property.SetValue(importedMed, null);
                            continue;
                        }

                        if (property != null && property.CanWrite)
                        {
                            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var safeValue = (propertyValue == null) ? null : Convert.ChangeType(propertyValue, targetType);
                            property.SetValue(importedMed, safeValue);
                        }
                    }

                    OnProgressChanged?.Invoke(sheet.LastRowNum + 1, i, importedMed.Denumire);

                    await new ImportedMedRepository().AddIfNotExists(importedMed);
                }

                var rowCount = sheet.PhysicalNumberOfRows;
            }

            return true;
        }
    }
}
