using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NPOI.SS.Formula.Functions;
using VetManagement.Commands;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class ImportedMedsImportingViewModel : ViewModelBase
    {
        private readonly ImportedMedsFileHelper _importedMedsFileHelper;

        private int _totalMeds;
        public int TotalMeds
        {
            get => _totalMeds;
            set
            {
                _totalMeds = value;
                OnPropertyChanged(nameof(TotalMeds));
            }
        }

        private int _currentMedIndex;
        public int CurrentMedIndex
        {
            get => _currentMedIndex;
            set
            {
                _currentMedIndex = value;
                OnPropertyChanged(nameof(CurrentMedIndex));

            }
        }

        private string _currentMedName;
        public string CurrentMedName
        {
            get => _currentMedName;
            set
            {
                _currentMedName = value;
                OnPropertyChanged(nameof(CurrentMedName));

            }
        }

        private float _progressFiller;
        public float ProgressFiller
        {
            get => _progressFiller;
            set
            {
                _progressFiller = value;
                OnPropertyChanged(nameof(ProgressFiller));
            }
        }

        private bool _isProcessing = false;

        private readonly string _filePath;

        private readonly Func<Task>? OnMedsImported;

        private CancellationTokenSource _cancellationTokenSource;

        public ICommand CancelImportCommand { get; set; }

        public ImportedMedsImportingViewModel(NavigationStore navigationStore, string filePath, Func<Task>? onMedsImported) 
        { 
            _navigationStore = navigationStore;
            _filePath = filePath;
            OnMedsImported = onMedsImported;

            OnLoadedCommand = new RelayCommand(ImportMeds);
            CancelImportCommand = new RelayCommand(StopImporting, (object param) => _isProcessing);

            _importedMedsFileHelper = new ImportedMedsFileHelper(UpdateProgress);

        }

        private async void ImportMeds(object sender)
        {
            _isProcessing = true;

            if (string.IsNullOrEmpty(_filePath))
            {
                Boxes.Warning("Nu a fost găsită calea către fișier!");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                bool res = await _importedMedsFileHelper.ImportMeds(_filePath, token);

                if (res)
                {
                    OnMedsImported?.Invoke();

                    Boxes.InfoBox("Medicamentele au fost importate cu succes!");
                }
            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare in citirea fișierului!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                _isProcessing = false;
                new CloseWindowCommand<ImportedMedsImportingViewModel>
                          (new WindowService<ImportedMedsImportingViewModel>(_navigationStore, null), this);
            }

        }

        public void StopImporting(object parameter)
        {
            var response = Boxes.ConfirmBox("Sunteți sigur că doriți să opriți procesul?");

            if ( response == System.Windows.MessageBoxResult.Yes ) 
            {
                if( _cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        private void UpdateProgress(int totalMeds, int currentMedIndex, string currentMedName)
        {
            //Trace.WriteLine(CurrentMedIndex);

            TotalMeds = totalMeds;
            CurrentMedIndex = currentMedIndex;
            CurrentMedName = currentMedName;

            ProgressFiller = ((float)CurrentMedIndex / TotalMeds) * 100;

        }

    }
}
