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

namespace VetManagement.Services
{
    public class ImportedProductsFileHelper
    {

        public Action<int,int,string> OnProgressChanged;

        public ImportedProductsFileHelper(Action<int, int, string> onProgressChanged)
        {
            OnProgressChanged = onProgressChanged;
        }

        public async Task<bool> ImportProducts(string path)
        {
            //path = "C:\\Users\\eduar\\Desktop\\produse.xls";

            if (!File.Exists(path))
            {
                Boxes.InfoBox("Fișierul nu a fost găsit!");
                return false;
            }

            List<string> importedProductProperties = ObjectHelper.GetPropertiesAsStrings(typeof(ImportedProduct));

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var workbook = new HSSFWorkbook(fs);
                var sheet = workbook.GetSheetAt(0);

                var firstRow = sheet.GetRow(0);

                var collCount = firstRow.LastCellNum;

                if (collCount != importedProductProperties.Count())
                {
                    Boxes.InfoBox("Fișierul nu coincide cu șablonul de bază!");
                    Logger.LogError("Error", "IMPORTED PRODUCTS ERROR: Column count not matching!\n" +
                        "File:" + collCount + "\n" +
                        "Database: " + importedProductProperties.Count());
                    return false;
                }

                for (int i = 0; i < collCount; i++)
                {
                    var cell = firstRow.GetCell(i);

                    if (cell.ToString() != importedProductProperties[i])
                    {
                        Boxes.InfoBox("Fișierul nu coincide cu șablonul de bază!");
                        Logger.LogError("Error", "IMPORTED PRODUCTS ERROR: Column count not matching!\n" +
                           "File column:" + cell.ToString() + "\n" +
                           "Database column: " + importedProductProperties[i]);
                        return false;
                    }
                }

                for (int i = 1; i < sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var importedProduct = new ImportedProduct();

                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        var propertyName = importedProductProperties[j];
                        var propertyValue = row.GetCell(j).ToString();
                        var property = typeof(ImportedProduct).GetProperty(propertyName);

                        if( string.IsNullOrEmpty(propertyValue))
                        {
                            property.SetValue(importedProduct, null);
                            continue;
                        }

                        if (property != null && property.CanWrite)
                        {
                            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var safeValue = (propertyValue == null) ? null : Convert.ChangeType(propertyValue, targetType);
                            property.SetValue(importedProduct, safeValue);
                        }
                        
                    }
                    OnProgressChanged?.Invoke(sheet.LastRowNum + 1, i, importedProduct.Denumire);

                    await new BaseRepository<ImportedProduct>().Add(importedProduct);
                }

                var rowCount = sheet.PhysicalNumberOfRows;
            }

            return true;
        }
    }
}
