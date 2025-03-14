using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace VetManagement.Services
{
    public class BrowseFileHelper
    {

        public static string BrowseExcelFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            bool? userClickedOK = openFileDialog.ShowDialog();
            if (userClickedOK == true)
            {
                return openFileDialog.FileName;
            }
            return string.Empty;
        }
    }
}
