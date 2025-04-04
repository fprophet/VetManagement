using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Mysqlx.Prepare;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedReportsViewModel : ViewModelBase
    {

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

                //GetTreatments();
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

                //GetTreatments();
            }
        } 
        
        private string _treatmentTypeFilter = "";
        public string TreatmentTypeFilter
        {
            get => _treatmentTypeFilter;
            set
            {
                _treatmentTypeFilter = value;

                if( !string.IsNullOrEmpty(MedName) || !string.IsNullOrEmpty(OwnerName) || FromDate != null)
                {
                    IsLoading = true;
                    _filterService.DebouncePropertyChanged(nameof(TreatmentTypeFilter));
                }

                //GetTreatments();
            }
        }

        public ObservableCollection<object> TreatmentTypeList { get; set; } =
            new ObservableCollection<object> { new { Name = "Toate", Value = "" }, new { Name = "Animale mici", Value = "pet" }, new { Name = "Animale mari", Value = "livestock" } };

        private DateTime? _fromDate = null;
        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                _filterService.DebouncePropertyChanged(nameof(FromDate));

            }
        }
        
        private DateTime? _toDate = null;
        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                Trace.WriteLine(value);

                if (!string.IsNullOrEmpty(MedName) || !string.IsNullOrEmpty(OwnerName) || FromDate != null)
                {
                    IsLoading = true;
                    _filterService.DebouncePropertyChanged(nameof(ToDate));
                }

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
        public ObservableCollection<TreatmentWrapper> TreatmentWrappers { get; } = new ObservableCollection<TreatmentWrapper>();

        public ICommand ExportToCSVCommand { get; }
        public ICommand PrintCommand { get; }

        public MedReportsViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "📊 Rapoarte medicamente";
            _filterService = new FilterService(GetTreatments);

            PaginationService = new PaginationService(GetTreatments, GetTreatments,100);

            ExportToCSVCommand = new RelayCommand(ExportToCSV, CanExportToCsv);
            PrintCommand = new RelayCommand(Print);
        }


        private void Print(object parameter)
        {
            PrintDialog printDialog = new PrintDialog();

            printDialog.ShowDialog();
        }

        private bool CanExportToCsv(object parameter)
        {
            return PaginationService.TotalFound != null && PaginationService.TotalFound > 0;
        }

        private async void ExportToCSV(object parameter)
        {
            if( parameter is not string)
            {
                return;
            }

            string type = (string)parameter;

            List<Treatment> Treatments = new List<Treatment>();

            if( type == "page")
            {
                foreach (TreatmentWrapper treatmentWrapper in TreatmentWrappers)
                {
                    Treatments.Add(treatmentWrapper.Treatment);
                }
            }
            else
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters.Add("medName", MedName);
                filters.Add("fromDateFilter", FromDate);
                filters.Add("toDateFilter", ToDate);
                filters.Add("ownerName", OwnerName);
                filters.Add("patientType", TreatmentTypeFilter);
                var (treatments, totalFound) = await _treatmentRepository.GetFullTreatments(PaginationService.PageNumber, PaginationService.TotalFound, filters);

                Treatments = treatments;
            }

            ExportCSVHelper.ExportReports(Treatments);
            
        }

        private async Task GetTreatments()
        {
            TreatmentWrappers.Clear();
            IsLoading = true;
            try
            {

                if (string.IsNullOrEmpty(MedName) && FromDate == null && string.IsNullOrEmpty(OwnerName))
                {
                    return;
                }


                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters.Add("medName", MedName);
                filters.Add("fromDateFilter", FromDate);
                filters.Add("toDateFilter", ToDate);
                filters.Add("ownerName", OwnerName);
                filters.Add("patientType", TreatmentTypeFilter);
                Trace.WriteLine(TreatmentTypeFilter);

                var (treatments, totalFound) = await _treatmentRepository.GetFullTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);

                PaginationService.TotalFound = totalFound;

                foreach (Treatment treatment in treatments)
                {
                    TreatmentWrapper tr = new TreatmentWrapper(treatment);
                    //Trace.WriteLine(tr.Meds.GetType().GetGenericArguments()[0]);
                    TreatmentWrappers.Add( tr);
                }

                if (TreatmentWrappers.Count() == 0)
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
                if(TreatmentWrappers.Count > 0 )
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
