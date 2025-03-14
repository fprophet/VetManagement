using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class OwnersViewModel : ViewModelBase
    {

        private string _pageTitle = "👤 Pagină proprietari";

        public ICommand NavigateCreateOwnerWindowCommand { get; set; }

        public ICommand DeleteOwnerCommand { get; set; }

        public ICommand NavigateTreatmentsListCommand { get; set; }

        public ICommand NavigatePatientsListCommand { get; set; }

        public ICommand UpdateOwnerCommand { get; set; }

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

        public ObservableCollection<Owner> Owners { get; set; } = new ObservableCollection<Owner>();


        private string _nameFilter = "";
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                isLoading = true;
                Owners.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(NameFilter));
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
                Owners.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(PatientNameFilter));
            }
        }

        private string _detailsFilter = "";
        public string DetailsFilter
        {
            get => _detailsFilter;
            set
            {
                _detailsFilter = value;
                isLoading = true;
                Owners.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(DetailsFilter));
            }
        }

        public OwnersViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;


            _filterService = new FilterService(() => LoadOwners());

            PaginationService = new PaginationService(() => LoadOwners(), () => LoadOwners(),20);

            NavigateCreateOwnerWindowCommand = new NavigateWindowCommand<CreateOwnerAndPatientViewModel>
                (new WindowService<CreateOwnerAndPatientViewModel>(_navigationStore, (id) => new CreateOwnerAndPatientViewModel(_navigationStore, UpdateOwners)), () => new CreateOwnerAndPatientWindow());

            NavigateTreatmentsListCommand = new NavigateCommand<OwnerTreatmentsViewModel>
                (new NavigationService<OwnerTreatmentsViewModel>(_navigationStore, (id) => new OwnerTreatmentsViewModel(_navigationStore, id)));

            NavigatePatientsListCommand = new NavigateCommand<OwnerPatientsViewModel>
                (new NavigationService<OwnerPatientsViewModel>(_navigationStore, (id) => new OwnerPatientsViewModel(_navigationStore, id)));

            DeleteOwnerCommand = new RelayCommand(DeleteOwner);
            UpdateOwnerCommand = new RelayCommand(UpdateOwner);

        }

        public async Task LoadOwners()
        {
            Owners.Clear();
            try
            {
                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters["nameFilter"] = NameFilter;
                filters["patientNameFilter"] = PatientNameFilter;
                filters["detailsFIlter"] = DetailsFilter;

                var (owners, totalRecords) = await new OwnerRepository().GetFullInfoFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);
                //var owners = await new OwnerRepository().GetFullInfo();

                PaginationService.TotalFound = totalRecords;

                var sortedOwners = owners.OrderByDescending(o => o.DateAdded);

                foreach (var owner in sortedOwners)
                {
                    Owners.Add(owner);
                }


            } catch (Exception ex)
            {
                Boxes.ErrorBox("Eroare in redarea listei de proprietari!\n" + ex.Message);
            }finally
            {
                isLoading = false;
            }
        }

        public void UpdateOwners(Owner owner)
        {

            Owners.Add(owner);

            var sortedOwners = Owners.OrderByDescending(o => o.DateAdded).ToList();

            Owners.Clear();

            foreach( var sorted in sortedOwners)
            {
                Owners.Add(sorted);
            }
        }

        private async void UpdateOwner(object parameter)
        {
            try
            {
                if (parameter != null && parameter is Owner)
                { 
                    Owner owner = (Owner)parameter;
                    await new OwnerRepository().Update(owner);
                    Boxes.InfoBox("Proprietarul a fost actualizat!");

                }
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Proprietarul nu a putut fi actualizat!\n" + e.Message);
            }
        }

        private async void DeleteOwner(object parameter)
        {
            try 
            {
                var result = Boxes.ConfirmBox("Doriți să ștergeți proprietarul?");
                if (parameter is int && result == MessageBoxResult.Yes) 
                {
                    await new OwnerRepository().Delete((int)parameter);

                    var owner = Owners.FirstOrDefault(o => o.Id == (int)parameter);

                }
                else 
                {
                    Boxes.ErrorBox("Proprietarul nu a putut fi șters! Id  invalid!" );
                }
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Proprietarul nu a putut fi șters!\n" + e.Message);
            }
        }
    }
}
