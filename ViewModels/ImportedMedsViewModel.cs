using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;


namespace VetManagement.ViewModels
{
    public class ImportedMedsViewModel : ViewModelBase
    {
        public ICommand ImportMedsCommand { get; }
        public ICommand BrowseFileCommand { get; }
        public ICommand OnGenerateColumnsCommand { get; }
        public ICommand OnLayoutUpdatedCommand { get; }
        public ICommand DeleteImportedMedCommand { get; }

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

        private string _nameFilter = "";
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(NameFilter));
            }
        }

        private string _idFilter = "";
        public string IdFilter
        {
            get => _idFilter;
            set
            {
                _idFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(IdFilter));
            }
        }

        private string _codeFilter = "";
        public string CodeFilter
        {
            get => _codeFilter;
            set
            {
                _codeFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(CodeFilter));
            }
        }

        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is ImportedMed)
                {
                    SelectedImportedMoed = (ImportedMed)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private ImportedMed _selectedImportedMed;
        public ImportedMed SelectedImportedMoed
        {
            get => _selectedImportedMed;
            set
            {
                _selectedImportedMed = value;
                OnPropertyChanged(nameof(SelectedImportedMoed));
            }
        }

        private readonly FilterService _filterService;
        public FilterHelper FilterHelper { get; set; } = new FilterHelper();

        public ImportedMedsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📦 Medicamente Importate";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadImportedMeds());

            _filterService = new FilterService(LoadImportedMeds);

            PaginationService = new PaginationService(LoadImportedMeds, LoadImportedMeds, 20);

            DeleteImportedMedCommand = new RelayCommand(DeleteImportedMed, CanExecuteMedAction);
            ImportMedsCommand = new RelayCommand(ImportMeds, (object parameter) => !string.IsNullOrEmpty(FilePath));

            BrowseFileCommand = new RelayCommand(BrowseFile);
            OnGenerateColumnsCommand = new RelayCommand(OnGenerateColumns);
        }
        
        public bool CanExecuteMedAction(object parameter) => _selectedImportedMed != null && _selectedImportedMed.Id is string;

        private async void DeleteImportedMed(object parameter)
        {
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa ștergeți acest medicament: " + _selectedImportedMed.Denumire + "?\n Tratamentele aferente acestui medicament vor fi arhivate!");

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            if (_selectedImportedMed != null)
            {
                try
                {
                    await ImportedMedService.Delete(_selectedImportedMed);
                    //await new BaseRepository<Med>().Delete(_selectedMed.Id);
                    Boxes.InfoBox("Medicamentul a fost șters!");
                }
                catch (Exception e)
                {
                    Boxes.ErrorBox("Probleme în ștergerea medicamentului!\n" + e.Message);
                    return;
                }

                var med = ImportedMeds.FirstOrDefault(p => p.Id == _selectedImportedMed.Id);
                if (med != null)
                {
                    ImportedMeds.Remove(med);
                }
                _selectedImportedMed = null;

            }
            else
            {
                Boxes.ErrorBox("Probleme în ștergerea medicamentului! No ID found!");
            }
        }

        private void OnGenerateColumns(object parameter)
        {
            Trace.WriteLine("yeeeeeeeeeeeeeeeeeeee");
        }

        private void BrowseFile(object parameter)
        {
            FilePath = BrowseFileHelper.BrowseExcelFile();
            ImportMeds(null);
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

                filters.Add("nameFilter", NameFilter);  
                filters.Add("idFilter", IdFilter);  
                filters.Add("codeFilter", CodeFilter);  
              
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
