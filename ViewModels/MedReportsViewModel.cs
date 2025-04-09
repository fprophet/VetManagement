using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Mysqlx.Prepare;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
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
        public ObservableCollection<TreatmentDisplay> TreatmentsDisplay { get; } = new ObservableCollection<TreatmentDisplay>();

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

            List<TreatmentDisplay> toExport = new List<TreatmentDisplay>();

            if ( type == "page")
            {
                foreach (TreatmentDisplay treatmentDisplate in TreatmentsDisplay)
                {
                    toExport.Add(treatmentDisplate);
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
                var (treatments, totalFound) = await _treatmentRepository.GetCurrentAndHistoryTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);


                toExport = treatments;
            }

            ExportCSVHelper.ExportReports(toExport);
            
        }

        private async Task GetTreatments()
        {
            TreatmentsDisplay.Clear();
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


                await Task.Run(async () =>
                {
                    var (treatments, totalFound) = await _treatmentRepository.GetCurrentAndHistoryTreatments(PaginationService.PageNumber, PaginationService.PerPage, filters);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PaginationService.TotalFound = totalFound;
                        if (PaginationService.TotalFound > 0)
                        {
                            NoResults = false;
                        }
                        else
                        {
                            NoResults = true;

                        }

                    });

                    foreach (TreatmentDisplay treatment in treatments)
                    {

                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            TreatmentsDisplay.Add(treatment);
                        }, DispatcherPriority.Background);
                    }

                });

            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au fost găsite!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
            finally
            {
                Trace.WriteLine(TreatmentsDisplay.Count);
                IsLoading = false;
           

            }

        }

      
    }
}
