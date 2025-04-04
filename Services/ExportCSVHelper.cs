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

        public static void ExportReports(List<Treatment> List)
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

        private static void PopulateReprotsCSVFile(string Path, List<Treatment> Items, List<string> Columns)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Tratamente");

            IRow headerRow = sheet.CreateRow(0);

            int colIdx = 0;
            foreach (string colName in Columns)
            {
                headerRow.CreateCell(colIdx).SetCellValue(colName);
                Trace.WriteLine(colName);
                colIdx++;
            }

            int rowCount = 1;
            foreach (Treatment treatment in Items)
            {

                if (treatment.TreatmentMeds != null && treatment.TreatmentMeds.Count > 0)
                {
                    foreach (var med in treatment.TreatmentMeds)
                    {
                        IRow row = sheet.CreateRow(rowCount);

                        row.CreateCell(0).SetCellValue(treatment.DateAdded.Date.ToString("yyyy-MM-dd"));
                        row.CreateCell(1).SetCellValue(treatment.Owner.Name);
                        row.CreateCell(2).SetCellValue(treatment.Owner.Address);
                        row.CreateCell(3).SetCellValue(Convert.ToString(treatment.Patient.Identifier));
                        row.CreateCell(4).SetCellValue(treatment.Patient.Species);
                        row.CreateCell(5).SetCellValue(treatment.Patient.Race);
                        row.CreateCell(6).SetCellValue(treatment.Patient.Sex);
                        row.CreateCell(7).SetCellValue(treatment.Patient.Age);
                        row.CreateCell(8).SetCellValue(treatment.Patient.Weight);

                        TreatmentMed tm = (TreatmentMed)med;
                        row.CreateCell(9).SetCellValue(tm.Med.Name);
                        row.CreateCell(10).SetCellValue(tm.Med.LotID);
                        row.CreateCell(11).SetCellValue(tm.Med.Valability);
                        row.CreateCell(12).SetCellValue(Convert.ToString(tm.Quantity));
                        rowCount++;
                    }
                }
                else if (treatment.TreatmentImportedMeds != null && treatment.TreatmentImportedMeds.Count > 0)
                {
                    foreach (var med in treatment.TreatmentImportedMeds)
                    {
                        IRow row = sheet.CreateRow(rowCount);

                        row.CreateCell(0).SetCellValue(treatment.DateAdded.Date.ToString("yyyy-MM-dd"));
                        row.CreateCell(1).SetCellValue(treatment.Owner.Name);
                        row.CreateCell(2).SetCellValue(treatment.Owner.Address);
                        row.CreateCell(3).SetCellValue(Convert.ToString(treatment.Patient.Identifier));
                        row.CreateCell(4).SetCellValue(treatment.Patient.Species);
                        row.CreateCell(5).SetCellValue(treatment.Patient.Race);
                        row.CreateCell(6).SetCellValue(treatment.Patient.Sex);
                        row.CreateCell(7).SetCellValue(treatment.Patient.Age);
                        row.CreateCell(8).SetCellValue(treatment.Patient.Weight);

                        TreatmentImportedMed tim = (TreatmentImportedMed)med;
                        row.CreateCell(9).SetCellValue(tim.ImportedMed.Denumire);
                        row.CreateCell(10).SetCellValue("");
                        row.CreateCell(11).SetCellValue("");
                        row.CreateCell(12).SetCellValue(tim.Quantity );
                        rowCount++;
                    }
                }
            }

            using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
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

            if( Item.GetType().Name == "Treatment")
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
