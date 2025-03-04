using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VetManagement.ViewModels
{
    public class RegistryViewModel : ViewModelBase
    {

        public ObservableCollection<RegistryRecord> RegistryRecords { get; } = new ObservableCollection<RegistryRecord>();

        private ListCollectionView _filteredRegistryRecords;

        public ListCollectionView FilteredRegistryRecords
        {
            get => _filteredRegistryRecords;
            set
            {
                _filteredRegistryRecords = value;
                OnPropertyChanged(nameof(FilteredRegistryRecords));
                FilteredRegistryRecords.Refresh();
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
                FilteredRegistryRecords.Refresh();
            }
        }

        private string _patientFilter;
        public string PatientFilter
        {
            get => _patientFilter;
            set
            {
                _patientFilter = value;
                OnPropertyChanged(nameof(PatientFilter));
                FilteredRegistryRecords.Refresh();
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
                FilteredRegistryRecords.Refresh();
            }
        }

        private string _identifierFilter;
        public string IdentifierFilter
        {
            get => _identifierFilter;
            set
            {
                _identifierFilter = value;
                OnPropertyChanged(nameof(IdentifierFilter));
                FilteredRegistryRecords.Refresh();
            }
        }


        public ICommand NavigateCreateRegisterRecordWindowCommand { get; set; }
        public RegistryViewModel(NavigationStore navigationStore) {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "Registru animale mari";

            FilteredRegistryRecords = new ListCollectionView(RegistryRecords);
            FilteredRegistryRecords.Filter = FilterRegistryRecords;


            NavigateCreateRegisterRecordWindowCommand = new NavigateWindowCommand<CreateRegistryRecordViewModel>
                (new NavigationService<CreateRegistryRecordViewModel>
                    (_navigationStore, (id) => new CreateRegistryRecordViewModel(_navigationStore, UpdateRegistryRecords)), () => new CreateRegistryRecordWindow());
        }
        

        private bool FilterRegistryRecords(object obj)
        {
            if (String.IsNullOrEmpty(OwnerNameFilter) && String.IsNullOrEmpty(PatientFilter) 
                && String.IsNullOrEmpty(MedNameFilter) && String.IsNullOrEmpty(IdentifierFilter))
            {
                return true;
            }
            else
            {
                var registryRecord = obj as RegistryRecord;

                bool ownerNameMatch = string.IsNullOrEmpty(OwnerNameFilter)
                    || (registryRecord?.Treatment?.Owner?.Name != null 
                            && (registryRecord.Treatment?.Owner?.Name.IndexOf(OwnerNameFilter, StringComparison.OrdinalIgnoreCase) >= 0));

                //bool patientMatch = string.IsNullOrEmpty(PatientFilter)
                //    || (registryRecord?.Species != null 
                //            && registryRecord.Species.IndexOf(PatientFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                bool medNameMatch = string.IsNullOrEmpty(MedNameFilter)
                    || (registryRecord?.Treatment?.TreatmentMeds != null &&
                        registryRecord.Treatment?.TreatmentMeds.Find(tm => tm.Med.Name.IndexOf(MedNameFilter, StringComparison.OrdinalIgnoreCase) >= 0) != null);

                //bool identifierMatch = string.IsNullOrEmpty(IdentifierFilter)
                //   || (registryRecord?.Identifier != null
                //           && registryRecord.Identifier.IndexOf(IdentifierFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                return ownerNameMatch  && medNameMatch ;
            }
        }

        private void UpdateRegistryRecords(RegistryRecord registryRecord)
        {
            RegistryRecords.Add(registryRecord);

            var sorted = RegistryRecords.OrderByDescending(rr => rr.Date);

            RegistryRecords.Clear();

            foreach( var rr in sorted)
            {
                RegistryRecords.Add(rr);
            }

        }

        public async Task LoadRegistryRecords()
        {
            try
            {
                RegistryRecordRepository registryRecordRepository = new RegistryRecordRepository();

                var registryRecords = await registryRecordRepository.GetRegistryRecords();

                var sorted = registryRecords.OrderByDescending(r => r.Date);

                Trace.WriteLine("VEIRICARE");
                foreach( RegistryRecord registryRecord in sorted)
                {
                    RegistryRecords.Add(registryRecord);
                    Trace.WriteLine(registryRecord.Treatment.Patient);
                    //Trace.WriteLine(registryRecord.Treatment.Patient.Identifier);
                    //Trace.WriteLine(registryRecord.Treatment.Patient.Age);

                }


            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Tratamentele nu au fost găsite!\n" + e.Message);
            }

        }
      
    }
}
