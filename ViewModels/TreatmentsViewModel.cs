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

namespace VetManagement.ViewModels
{
    public class TreatmentsViewModel : ViewModelBase
    {
        public string _pageTitle = "Tratamente";

        public ICommand NavigateOwnersCommand { get; set; }

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

        private string _ownerNameFilter;
        public string OwnerNameFilter
        {
            get => _ownerNameFilter;
            set
            {
                _ownerNameFilter = value;
                OnPropertyChanged(nameof(OwnerNameFilter));
                FilteredTreatments.Refresh();
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
                FilteredTreatments.Refresh();
            }
        }

        private string _medNameFilter;
        public string MedNameFilter
        {
            get => _medNameFilter;
            set
            {
                _medNameFilter = value;
                OnPropertyChanged(nameof(MedNameFilter));
                FilteredTreatments.Refresh();
            }
        }

        //public CreateTreatmentViewModel CreateTreatmentViewModel { get; set; }

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();

        private ListCollectionView _filteredTreatments;

        public ICollectionView FilteredTreatments
        {
            get => _filteredTreatments;

        }

        public TreatmentsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;


            _filteredTreatments = new ListCollectionView(Treatments);
            _filteredTreatments.Filter = FilterTreatments;


            NavigateOwnersCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, (id) => new HomeViewModel(_navigationStore)));
            //CreateTreatmentViewModel = new CreateTreatmentViewModel(OnTreatmentCreated, id);

        }

        private bool FilterTreatments(object obj)
        {
            Trace.WriteLine("AICI");
            if (String.IsNullOrEmpty(OwnerNameFilter) && String.IsNullOrEmpty(PatientNameFilter) && String.IsNullOrEmpty(MedNameFilter))
            {
                return true;
            }
            else
            {
                var treatment = obj as Treatment;

                bool ownerNameMatch = string.IsNullOrEmpty(OwnerNameFilter)
                    || (treatment?.Owner?.Name != null && (treatment?.Owner?.Name.IndexOf(OwnerNameFilter, StringComparison.OrdinalIgnoreCase) >= 0));

                bool patientNameMatch = string.IsNullOrEmpty(PatientNameFilter)
                    || (treatment?.Patient?.Name != null && treatment.Patient.Name.IndexOf(PatientNameFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                bool medNameMatch = string.IsNullOrEmpty(MedNameFilter)
                    || (treatment?.TreatmentMeds != null &&
                        treatment?.TreatmentMeds.Find(tm => tm.Med.Name.IndexOf(PatientNameFilter, StringComparison.OrdinalIgnoreCase) >= 0) != null ) ;

                return ownerNameMatch && patientNameMatch && medNameMatch;
            }
        }

        private void OnTreatmentCreated(Treatment treatment)
        {
            Treatments.Add(treatment);
            var sortedTreatments = Treatments.OrderByDescending(t => t.DateAdded).ToList();

            Treatments.Clear();

            foreach (var sorted in sortedTreatments)
            {
                Treatments.Add(sorted);
            }
            FilteredTreatments.Refresh();
        }


        public async Task LoadTreatments()
        {
            try
            {
                var treatments = await new TreatmentRepository().GetFullTreatments();

                var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);

                foreach (var treatment in sortedTreatments)
                {

                    Treatments.Add(treatment);
                }
                isLoading = false;
            }catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au putut fi găsite!\n" + e.Message);
            }


        }
    }
}
