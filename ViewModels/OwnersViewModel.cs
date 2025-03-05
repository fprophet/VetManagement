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

        private string _pageTitle = "Pagină proprietari";

        public ICommand NavigateCreateOwnerWindowCommand { get; set; }

        public ICommand DeleteOwnerCommand { get; set; }

        public ICommand NavigateTreatmentsListCommand { get; set; }

        public ICommand NavigatePatientsListCommand { get; set; }

        public ICommand UpdateOwnerCommand { get; set; }


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

        private ListCollectionView _filteredOwners;

        public ICollectionView FilteredOwners
        {
            get => _filteredOwners;

        }

        private string _nameFilter;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
                FilteredOwners.Refresh();
            }
        }

        private string _patientNameFilter;
        public string PatientNameFilter
        {
            get => _patientNameFilter;
            set
            {
                _patientNameFilter = value;
                OnPropertyChanged(nameof(PatientNameFilter));
                FilteredOwners.Refresh();
            }
        }

        private string _detailsFilter;
        public string DetailsFilter
        {
            get => _detailsFilter;
            set
            {
                _detailsFilter = value;
                OnPropertyChanged(nameof(DetailsFilter));
                FilteredOwners.Refresh();
            }
        }

        public OwnersViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            _filteredOwners = new ListCollectionView(Owners);
            _filteredOwners.Filter = FilterOwners;

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
            try
            {
                var owners = await new OwnerRepository().GetFullInfo();

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

        private bool FilterOwners(object obj)
        {
            if (String.IsNullOrEmpty(NameFilter) && String.IsNullOrEmpty(PatientNameFilter) && String.IsNullOrEmpty(DetailsFilter))
            {
                return true;
            }
            else
            {
                var owner = obj as Owner;

                bool nameMatch = string.IsNullOrEmpty(NameFilter) 
                    || (owner?.Name != null && owner.Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                bool patientNameMatch = string.IsNullOrEmpty(PatientNameFilter) || owner?.Patients
                    .Find(p => p.Name != null && p.Name.IndexOf(PatientNameFilter, StringComparison.OrdinalIgnoreCase) >= 0) != null;

                bool detailsMatch = string.IsNullOrEmpty(DetailsFilter)
                     || (owner?.Details != null && owner.Details.IndexOf(DetailsFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                return nameMatch && patientNameMatch && detailsMatch;
                   
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
