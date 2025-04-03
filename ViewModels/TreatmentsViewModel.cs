using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
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

        public ICommand NavigateCreateTreatmentWindowCommand { get; set; }
        
        public ICommand NavigateTreatmentViewCommand { get; set; }

        public PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;
        public FilterHelper FilterHelper { get; set; } = new FilterHelper();


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

        private string _patientSpeciesFilter = "";
        public string PatientSpeciesFilter
        {
            get => _patientSpeciesFilter;
            set
            {
                _patientSpeciesFilter = value;
                isLoading = true;
                Treatments.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(PatientSpeciesFilter));

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

        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is Treatment)
                {
                    _selectedTreatment = (Treatment)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private Treatment _selectedTreatment;
        public Treatment SelectedTreatment
        {
            get => _selectedTreatment;
            set
            {
                _selectedTreatment = value;
                OnPropertyChanged(nameof(SelectedTreatment));
            }
        }

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();

        public TreatmentsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            _filterService = new FilterService(() => LoadTreatments());

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadTreatments());

            PaginationService = new PaginationService(() => LoadTreatments(), () => LoadTreatments());

            NavigateTreatmentViewCommand = new NavigateCommand<TreatmentViewModel>
                (new NavigationService<TreatmentViewModel>(_navigationStore, (id) => new TreatmentViewModel(navigationStore, SelectedTreatment.Id)), CanExecuteTreatmentAction);

            NavigateCreateTreatmentWindowCommand = new NavigateWindowCommand<CreateTreatmentViewModel>
                (new WindowService<CreateTreatmentViewModel>(_navigationStore, (id) => new CreateTreatmentViewModel(_navigationStore, OnTreatmentCreated, null, null)), () => new CreateTreatmentWindow());
        }

        public bool CanExecuteTreatmentAction(object parameter) => SelectedTreatment != null && SelectedTreatment.Id is int;

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
            isLoading = true;
            Treatments.Clear();
            try
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters["ownerName"] = OwnerNameFilter;
                filters["patientSpecies"] = PatientSpeciesFilter;
                filters["medName"] = MedNameFilter;
                filters["patientType"] = "pet";

                await Task.Run(async () =>
                {
                    var (treatments, totalRecords) = await new TreatmentRepository().GetFullTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);


                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PaginationService.TotalFound = totalRecords;
                    });

                    var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);


                    foreach (var treatment in sortedTreatments)
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Treatments.Add(treatment);

                        }, DispatcherPriority.Background);
                    }
                });


               

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
