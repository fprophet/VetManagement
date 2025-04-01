using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Core;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Migrations;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace VetManagement.ViewModels
{
    public class ImportedMedsViewModel : ViewModelBase
    {
        public ICommand ImportMedsCommand { get; }
        public ICommand BrowseFileCommand { get; }
        public ICommand OnGenerateColumnsCommand { get; }
        public ICommand OnLayoutUpdatedCommand { get; }


        public ObservableCollection<ImportedMed> ImportedMeds { get; set; } = new();
        //public BindingList<ImportedMed> ImportedMeds { get; set; } = new();
  

        public PaginationService PaginationService { get; }

        private bool _isLoading = true;
        public bool isLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }
        
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public ImportedMedsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📦 Medicamente Importate";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadImportedMeds());

            PaginationService = new PaginationService(LoadImportedMeds, LoadImportedMeds, 20);

            ImportMedsCommand = new RelayCommand(ImportMeds, (object parameter) => !string.IsNullOrEmpty(FilePath));

            BrowseFileCommand = new RelayCommand(BrowseFile);
            OnGenerateColumnsCommand = new RelayCommand(OnGenerateColumns);
        }

        private void OnGenerateColumns(object parameter)
        {
            Trace.WriteLine("yeeeeeeeeeeeeeeeeeeee");
        }

        private void BrowseFile(object parameter)
        {
            FilePath = BrowseFileHelper.BrowseExcelFile();
        }

        private void ImportMeds(object parameter)
        {
            new NavigateWindowCommand<ImportedMedsImportingViewModel>
                (new WindowService<ImportedMedsImportingViewModel>
                    (_navigationStore, (id) => new ImportedMedsImportingViewModel(_navigationStore, FilePath, LoadImportedMeds)),
                    () => new ImportedMedsImportingWindow(), true);
        }

        public async Task LoadImportedMeds()
        {
            isLoading = true;
            ImportedMeds.Clear();
            try
            {
                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters.Add("nameFilter", "");

              
                await Task.Run(async () =>
                {
                    var (importedMeds, totalRecords) = await new ImportedMedRepository().GetAllFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PaginationService.TotalFound = totalRecords;
                    });
                    foreach (var med in importedMeds)
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            ImportedMeds.Add(med);
                        }, DispatcherPriority.Background);
                    }
                });
            }

            catch (Exception e)
            {
                MessageBox.Show("Lista de medicamente importate nu a putut fi redată!\n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isLoading = false;
            }



        }
    }
}
