using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedReportsViewModel : ViewModelBase
    {

        private NavigationStore _navigationStore;

        private TreatmentRepository _treatmentRepository = new TreatmentRepository();

        private string _medName;
        public string MedName
        {
            get => _medName;
            set
            {
                _medName = value;
                IsLoading = true;
                NoResults = false;
                Treatments.Clear();
                DebouncePropertyChanged(nameof(MedName));

                //GetNewMedList();
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
            _navigationStore.PageTitle = "Rapoarte medicamente";
        }

        private async void GetNewMedList()
        {
            var treatments = await  _treatmentRepository.GetByMedName(MedName);

            foreach ( Treatment treatment in treatments)
            {
                Treatments.Add(treatment);
            }

            if( Treatments.Count() == 0 )
            {
                NoResults = true;
            }
           

        }

        private CancellationTokenSource _debounceCts;
        private async void DebouncePropertyChanged([CallerMemberName] string propertyName = null)
        {
            _debounceCts?.Cancel(); // Cancel previous pending task
            _debounceCts = new CancellationTokenSource();
            var token = _debounceCts.Token;

            try
            {
                await Task.Delay(600, token);

                if (!token.IsCancellationRequested)
                {
                    Trace.WriteLine(IsLoading);

                    OnPropertyChanged(propertyName);
                    GetNewMedList();
                    IsLoading = false;
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore task cancellation
            }
        }
    }
}
