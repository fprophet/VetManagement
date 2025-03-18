using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VetManagement.ViewModels
{
    public class TreatmentsViewModel : ViewModelBase
    {
        public string _pageTitle = "💊 Registru animale mici";

        public ICommand NavigateOwnersCommand { get; set; }

        public ICommand NavigateCreateTreatmentWindowCommand { get; set; }
        
        public ICommand RepeatTreatmentCommand { get; set; }


        public PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;

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

        private string _ownerNameFilter = "";
        public string OwnerNameFilter
        {
            get => _ownerNameFilter;
            set
            {
                _ownerNameFilter = value;
                isLoading = true;
                Treatments.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(OwnerNameFilter));
            }
        }

        private string _patientNameFilter = "";
        public string PatientNameFilter
        {
            get => _patientNameFilter;
            set
            {
                _patientNameFilter = value;
                isLoading = true;
                Treatments.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(PatientNameFilter));

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
                Treatments.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(MedNameFilter));

            }
        }

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();

        public TreatmentsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            _filterService = new FilterService(() => LoadTreatments());

            PaginationService = new PaginationService(() => LoadTreatments(), () => LoadTreatments());

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment);

            NavigateOwnersCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, (id) => new HomeViewModel(_navigationStore)));
            
            NavigateCreateTreatmentWindowCommand = new NavigateWindowCommand<CreateTreatmentViewModel>
                (new WindowService<CreateTreatmentViewModel>(_navigationStore, (id) => new CreateTreatmentViewModel(_navigationStore, OnTreatmentCreated, null, null)), () => new CreateTreatmentWindow());
        }


        private async void RepeatTreatment(object parameter)
        {
            if (parameter is not int)
            {
                Boxes.InfoBox("Tratamentul nu a putut fi repetat!");
                return;
            }
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + parameter + "?");

            if (result == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            Treatment treatment = Treatments.FirstOrDefault(t => t.Id == (int)parameter);

            try
            {
                Treatment newTreatment = await DuplicateObjectService.DuplicateTreatment(treatment);
                OnTreatmentCreated(newTreatment);
            }
            catch (Exception e)
            {
                Logger.LogError("Error", e.ToString());
            }
        }

        private void OnTreatmentCreated(Treatment treatment)
        {
            if( treatment is null )
            {
                return;
            }

            Treatments.Add(treatment);
            var sortedTreatments = Treatments.OrderByDescending(t => t.Id).ToList();

            Treatments.Clear();

            foreach (var sorted in sortedTreatments)
            {
                Treatments.Add(sorted);
            }
        }


        public async Task LoadTreatments()
        {
            Treatments.Clear();
            try
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters["ownerName"] = OwnerNameFilter;
                filters["patientName"] = PatientNameFilter;
                filters["medName"] = MedNameFilter;
                filters["patientType"] = "pet";
                var (treatments, totalRecords) = await new TreatmentRepository().GetFullTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);

                PaginationService.TotalFound = totalRecords;

                var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);
                

                foreach (var treatment in sortedTreatments)
                {

                    Treatments.Add(treatment);
                }

            }catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au putut fi găsite!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                isLoading = false;
            }

        }
    }
}
