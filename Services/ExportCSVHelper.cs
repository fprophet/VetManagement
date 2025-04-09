using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using VetManagement.Data;
using VetManagement.DataWrappers;

namespace VetManagement.Services
{
    public class ExportCSVHelper
    {

        public static void ExportReports(List<TreatmentDisplay> List)
        {
            string FullPath = GetFilePath();

            if (string.IsNullOrEmpty(FullPath) || List.Count == 0)
            {
                return;
            }

            List<string> Columns = GetColumns(List[0]);

            try
            {
                PopulateReprotsCSVFile(FullPath,List, Columns);
            }
            catch(Exception e)
            {
                Logger.LogError("Error", e.ToString());
                Boxes.ErrorBox("Exportul nu a putut fi finalizat!\n" + e.Message);
                return;
            }

            Boxes.InfoBox("Exportul a fost realizat cu succes!");

        }

        private static void PopulateReprotsCSVFile(string Path, List<TreatmentDisplay> Items, List<string> Columns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Tratamente");

            IRow headerRow = sheet.CreateRow(0);

            int colIdx = 0;
            foreach (string colName in Columns)
            {
                headerRow.CreateCell(colIdx).SetCellValue(colName);
                colIdx++;
            }

            int rowCount = 1;
            foreach (TreatmentDisplay treatment in Items)
            {

                if (treatment.Meds != null && treatment.Meds.Count > 0)
                {
                    foreach (var med in treatment.Meds)
                    {
                        IRow row = sheet.CreateRow(rowCount);

                        row.CreateCell(0).SetCellValue(treatment.DateAdded.Date.ToString("yyyy-MM-dd"));
                        row.CreateCell(1).SetCellValue(treatment.OwnerName);
                        row.CreateCell(2).SetCellValue(treatment.OwnerAddress);
                        row.CreateCell(3).SetCellValue(Convert.ToString(treatment.PatientIdentifier));
                        row.CreateCell(4).SetCellValue(treatment.PatientSpecies);
                        row.CreateCell(5).SetCellValue(treatment.PatientRace);
                        row.CreateCell(6).SetCellValue(treatment.PatientSex);
                        row.CreateCell(7).SetCellValue(treatment.PatientAge);
                        row.CreateCell(8).SetCellValue(treatment.PatientWeight);

                        row.CreateCell(9).SetCellValue(med.Name);
                        row.CreateCell(10).SetCellValue(med.LotID);
                        row.CreateCell(11).SetCellValue(med.Valability.HasValue ? med.Valability.Value.ToString("yyyy-MM-dd") : string.Empty);
                        row.CreateCell(12).SetCellValue(Convert.ToString(med.Quantity));
                        rowCount++;
                    }
                }

                using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }

        private static string GetFilePath()
        {
            string DirectoryPath = GetDirectoryPath();

            if (string.IsNullOrEmpty(DirectoryPath))
            {
                return "";
            }

            return CreateFullPath(DirectoryPath);
        }


        private static List<string> GetColumns(object Item)
        {
            List<string> Columns = new List<string>();

            if( Item.GetType().Name == "TreatmentDisplay")
            {
                return ["Data","Proprietar","Adresa","Nume Animal/Numar Identificare","Specie","Rasa","Sex","Varsta","Greutate","Medicament","Serie","Valabiliate","Dozaj","Timp asteptare"];
            }

            return Columns;
        }

        private static string CreateFullPath(string directoryPath)
        {
            string date = DateTime.Now.Date.ToString("yyyy_MM_dd");

            string FullPath = directoryPath + Path.DirectorySeparatorChar + "tratamente_exportate_" + date + ".xls";

            int counter = 1;

            while (File.Exists(FullPath))
            {
                FullPath = directoryPath + Path.DirectorySeparatorChar + "tratamente_exportate_" + date + "_" + counter + ".xls";
                counter++;
            }

            return FullPath;
        }

        private static string GetDirectoryPath()
        {
            var dialog = new OpenFolderDialog() { Title = "Alege o locație" };

            if( dialog.ShowDialog() == true)
            {
                return dialog.FolderName;
            }

            return "";
        }
    }
}
