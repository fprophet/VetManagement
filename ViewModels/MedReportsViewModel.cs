using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Mysqlx.Prepare;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedReportsViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        public  PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;

        private TreatmentRepository _treatmentRepository = new TreatmentRepository();

        private string _medName;
        public string MedName
        {
            get => _medName;
            set
            {
                _medName = value;
                IsLoading = true;
                _filterService.DebouncePropertyChanged(nameof(MedName));

                //GetNewMedList();
            }
        }

        private string _ownerName;
        public string OwnerName
        {
            get => _ownerName;
            set
            {
                _ownerName = value;
                IsLoading = true;
                _filterService.DebouncePropertyChanged(nameof(OwnerName));

                //GetNewMedList();
            }
        }

        private DateTime? _date = null;
        public DateTime? Date
        {
            get => _date;
            set
            {
                _date = value;
                _filterService.DebouncePropertyChanged(nameof(Date));

            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        } 
        
        private bool _noResults = false;
        public bool NoResults
        {
            get => _noResults;
            set
            {
                _noResults = value;
                OnPropertyChanged(nameof(NoResults));
            }
        }

        public ObservableCollection<Med> Meds { get; } = new ObservableCollection<Med>();
        public ObservableCollection<Treatment> Treatments { get; } = new ObservableCollection<Treatment>();


        public MedReportsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📊 Rapoarte medicamente";
            _filterService = new FilterService(() => GetNewMedList());

            PaginationService = new PaginationService(() => GetNewMedList(), () => GetNewMedList(),100);
        }

        private async Task GetNewMedList()
        {
            Treatments.Clear();

            try
            {

                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters.Add("medName", MedName);
                filters.Add("dateFilter", Date);
                filters.Add("ownerName", OwnerName);

                var (treatments, totalFound) = await _treatmentRepository.GetFullTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);

                PaginationService.TotalFound = totalFound;

                foreach (Treatment treatment in treatments)
                {
                    Treatments.Add(treatment);
                }

                if (Treatments.Count() == 0)
                {
                    NoResults = true;
                }
            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au fost găsite!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                IsLoading = false;
                if(Treatments.Count > 0 )
                {
                    NoResults = false;
                }
                else
                {
                    NoResults = true;

                }

            }

        }

      
    }
}
