using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class RegistryViewModel : ViewModelBase
    {

        public ObservableCollection<RegistryRecord> RegistryRecords { get; } = new ObservableCollection<RegistryRecord>();


        private string _ownerNameFilter = "";
        public string OwnerNameFilter
        {
            get => _ownerNameFilter;
            set
            {
                _ownerNameFilter = value;
                PaginationService.PageNumber = 1;
                RegistryRecords.Clear();
                isLoading = true;
                _filterService.DebouncePropertyChanged(nameof(OwnerNameFilter));
            }
        }

        private string _patientFilter = "";
        public string PatientFilter
        {
            get => _patientFilter;
            set
            {
                _patientFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                RegistryRecords.Clear();
                _filterService.DebouncePropertyChanged(nameof(PatientFilter));
            }
        }

        private string _medNameFilter = "";
        public string MedNameFilter
        {
            get => _medNameFilter;
            set
            {
                _medNameFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                RegistryRecords.Clear();
                _filterService.DebouncePropertyChanged(nameof(MedNameFilter));
            }
        }

        private string _identifierFilter = "";
        public string IdentifierFilter
        {
            get => _identifierFilter;
            set
            {
                _identifierFilter = value;
                isLoading = true;
                PaginationService.PageNumber = 1;
                RegistryRecords.Clear();
                _filterService.DebouncePropertyChanged(nameof(IdentifierFilter));
            }
        }

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


        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is RegistryRecord)
                {
                    _selectedRegistryRecord = (RegistryRecord)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private RegistryRecord _selectedRegistryRecord;
        public RegistryRecord SelectedRegistryRecord
        {
            get => _selectedRegistryRecord;
            set
            {
                _selectedRegistryRecord = value;
                OnPropertyChanged(nameof(SelectedRegistryRecord));
            }
        }

        public PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;
        public FilterHelper FilterHelper { get; set; } = new FilterHelper();

        public ICommand NavigateCreateRegisterRecordWindowCommand { get; set; }

        public ICommand NavigateRegistryRecordViewCommand { get; set; }
        
        public RegistryViewModel(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📖 Registru animale mari";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadRegistryRecords());

            _filterService = new FilterService(LoadRegistryRecords);

            PaginationService = new PaginationService(LoadRegistryRecords, LoadRegistryRecords);

            NavigateRegistryRecordViewCommand = new NavigateCommand<RegistryRecordViewModel>
                (new NavigationService<RegistryRecordViewModel>(_navigationStore, (id) => new RegistryRecordViewModel(navigationStore, _selectedRegistryRecord.Id)), CanExecuteRegistryRecordAction);

            NavigateCreateRegisterRecordWindowCommand = new NavigateWindowCommand<CreateRegistryRecordViewModel>
                (new WindowService<CreateRegistryRecordViewModel>
                    (_navigationStore, (id) => new CreateRegistryRecordViewModel(_navigationStore, UpdateRegistryRecords)), () => new CreateRegistryRecordWindow());
        }

        public bool CanExecuteRegistryRecordAction(object parameter) => _selectedRegistryRecord != null && _selectedRegistryRecord.Id is int;


        private void UpdateRegistryRecords(RegistryRecord registryRecord)
        {
            if(registryRecord == null)
            {
                return;
            }

            RegistryRecords.Add(registryRecord);
          
            var sorted = RegistryRecords.OrderByDescending(rr => rr.Id).ToList();

            RegistryRecords.Clear();


            foreach ( RegistryRecord rr in sorted )
            {
                RegistryRecords.Add(rr);
            }

        }

        public async Task LoadRegistryRecords()
        {
            RegistryRecords.Clear();
            try
            {
                RegistryRecordRepository registryRecordRepository = new RegistryRecordRepository();

                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters["ownerNameFilter"] = OwnerNameFilter;
                filters["patientSpeciesFilter"] = PatientFilter;
                filters["medNameFilter"] = MedNameFilter;
                filters["identifierFilter"] = IdentifierFilter;

                //var registryRecords = await registryRecordRepository.GetRegistryRecords();
                var (registryRecords,totalRecords) = await registryRecordRepository.GetRegistryRecordsFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);

                PaginationService.TotalFound = totalRecords;

                var sorted = registryRecords.OrderByDescending(r => r.Date);

                foreach( RegistryRecord registryRecord in sorted)
                {
                    RegistryRecords.Add(registryRecord);
                }

            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au fost găsite!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                isLoading = false;
            }

        }
      
    }
}
