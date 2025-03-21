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

        public PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;

        public ICommand NavigateCreateRegisterRecordWindowCommand { get; set; }

        public ICommand RepeatTreatmentCommand { get; set; }
        
        public RegistryViewModel(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📖 Registru animale mari";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadRegistryRecords());

            _filterService = new FilterService(() => LoadRegistryRecords());

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment);

            PaginationService = new PaginationService(() => LoadRegistryRecords(), () => LoadRegistryRecords());

            NavigateCreateRegisterRecordWindowCommand = new NavigateWindowCommand<CreateRegistryRecordViewModel>
                (new WindowService<CreateRegistryRecordViewModel>
                    (_navigationStore, (id) => new CreateRegistryRecordViewModel(_navigationStore, UpdateRegistryRecords)), () => new CreateRegistryRecordWindow());
        }

        private async void RepeatTreatment(object parameter)
        {


            if (parameter is not int)
            {
                Boxes.InfoBox("Tratamentul nu a putut fi repetat!");
                return;
            }
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + parameter + "?");

            if( result == System.Windows.MessageBoxResult.No )
            {
                return;
            }

            RegistryRecord registryRecord = RegistryRecords.FirstOrDefault(rr => rr.Id == (int)parameter);

            try 
            { 
                RegistryRecord newRegistryRecord = await DuplicateObjectService.DuplicateRegistryRecord(registryRecord);

                if( newRegistryRecord is null)
                {
                    return;
                }

                UpdateRegistryRecords(newRegistryRecord);

                Notification Notification = new Notification()
                {
                    Type = "new-recipe",
                    Title = "A fost creată rețeta cu numărul:" + newRegistryRecord.RecipeNumber,
                    Message = "",
                    SentAt = DateTime.Now,
                    UserType = "user"
                };

                NotificationService.SendNotification(Notification);

            }
            catch (Exception e)
            {
                Logger.LogError("Error", e.ToString());
            }
        }


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
