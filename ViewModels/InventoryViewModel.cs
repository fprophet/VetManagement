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
using Google.Protobuf.WellKnownTypes;
using Mysqlx;
using Mysqlx.Crud;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private string _pageTitle = "Inventar";

        private readonly NavigationStore _navigationStore;

        public CreateMedViewModel CreateMedViewModel { get;  }

        public ICommand NavigateViewMedCommand { get; }

        public ICommand DeleteMedCommand { get; }

        public ICommand EditItemCommand { get; }

        public ICommand OpenCreateMedWindowCommand { get; }

        public ObservableCollection<Med> Meds { get; set; } = new ObservableCollection<Med>();

        public ObservableCollection<object> MedTypeList { get; set; } =
           new ObservableCollection<object> { new { Name = "Medicament", Value = "medicament" }, new { Name = "Vaccin", Value = "vaccin" } };

        private ListCollectionView _filteredMeds;

        public ListCollectionView FilteredMeds
        {
            get => _filteredMeds;
            set
            {
                _filteredMeds = value;
                OnPropertyChanged(nameof(FilteredMeds));
                FilteredMeds.Refresh();

            }
        }

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

        private string _typeFilter = "medicament";
        public string TypeFilter
        {
            get => _typeFilter;
            set
            {
                _typeFilter = value;
                OnPropertyChanged(nameof(TypeFilter));
                FilteredMeds.Refresh();
            }
        }  
        
        private string _nameFilter;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
                FilteredMeds.Refresh();
            }
        }

        private DateTime? _valabilityFilter;
        public DateTime? ValabilityFilter
        {
            get => _valabilityFilter;
            set
            {
                _valabilityFilter = value;
                OnPropertyChanged(nameof(ValabilityFilter));
                FilteredMeds.Refresh();
            }
        }

        private DateTime? _dateAddedFilter = null;
        public DateTime? DateAddedFilter
        {
            get => _dateAddedFilter;
            set
            {
                _dateAddedFilter = value;
                OnPropertyChanged(nameof(DateAddedFilter));
                FilteredMeds.Refresh();
            }
        }

        private string _lotFilter;
        public string LotFilter
        {
            get => _lotFilter;
            set
            {
                _lotFilter = value;
                OnPropertyChanged(nameof(LotFilter));
                FilteredMeds.Refresh();
            }
        }

        public InventoryViewModel(NavigationStore navigationStore) 
        { 
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            DeleteMedCommand = new RelayCommand(DeleteMed);
            EditItemCommand = new RelayCommand(UpdateMed);
            NavigateViewMedCommand = new NavigateCommand<MedViewModel>
                (new NavigationService<MedViewModel>(_navigationStore, (id) => new MedViewModel(_navigationStore, id)));
            
            OpenCreateMedWindowCommand = new NavigateWindowCommand<CreateMedViewModel>
                (new NavigationService<CreateMedViewModel>(_navigationStore, (id) => new CreateMedViewModel(UpdateMedList)), () => new CreateMedWindow() );
        }

        private bool FilterMeds(object obj)
        {
            if (String.IsNullOrEmpty(NameFilter) && String.IsNullOrEmpty(LotFilter) 
                && DateAddedFilter == default(DateTime) && ValabilityFilter == default(DateTime))
            {
                return true;
            }
            else
            {
                var med = obj as Med;

                bool nameMatch = string.IsNullOrEmpty(NameFilter)
                    || (med.Name != null && med.Name.IndexOf(NameFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                bool lotMatch = string.IsNullOrEmpty(LotFilter)
                    || (med.LotID != null && med.LotID.IndexOf(LotFilter, StringComparison.OrdinalIgnoreCase) >= 0);

                bool dateAddedMatch = DateAddedFilter == null ||
                    DateTimeOffset.FromUnixTimeSeconds(med.DateAdded).UtcDateTime.Date == DateAddedFilter;

                bool valabilityMatch = ValabilityFilter == null ||
                    DateTimeOffset.FromUnixTimeSeconds(med.Valability).UtcDateTime.Date == ValabilityFilter;

                bool typeMatch = TypeFilter == null || (med.Type != null && TypeFilter == med.Type);

                return nameMatch && lotMatch && dateAddedMatch && valabilityMatch && typeMatch;

            }
        }

        private void UpdateMedList(Med med)
        {

            Meds.Add(med);


            var sorted = Meds.OrderByDescending(m => m.DateAdded).ToList();
            
            Meds.Clear();

            foreach ( var itm in sorted)
            {
                Meds.Add(itm);
            }

            _filteredMeds = new ListCollectionView(Meds);

            FilteredMeds.Refresh();
        }

        private async void UpdateMed(object parameter)
        {
            try 
            { 
                if( parameter != null && parameter is Med)
                {
                    Med med = (Med)parameter;

                    med.TotalAmount = med.Pieces * med.PerPiece;

                    await new BaseRepository<Med>().Update(med);

                    Boxes.InfoBox("Medicamentul  a fost actualizat!");

                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Eroare in actualizarea medicamentului!\n" + e.Message);
            }

        }

        private async void  DeleteMed(object parameter)
        {
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa ștergeți acest medicament?");

            if (parameter is int && result == MessageBoxResult.Yes)
            {
                try
                {
                    await new BaseRepository<Med>().Delete((int)parameter);
                }
                catch (Exception e)
                { 
                    Boxes.ErrorBox("Probleme în ștergerea medicamentului!\n" + e.Message);
                    return;
                }

                var med = Meds.FirstOrDefault(p => p.Id == (int)parameter);
                if (med != null) 
                { 
                    Meds.Remove(med);
                }
            }
            else
            {
                Boxes.ErrorBox("Probleme în ștergerea medicamentului! No ID found!");
            }
        }

        public async Task LoadMeds() 
        {
            try
            {
                var meds = await new BaseRepository<Med>().GetAll();

                var sorted = meds.OrderByDescending(m => m.DateAdded);

                Meds.Clear();

                foreach (var med in sorted)
                {
                    Meds.Add(med);
                }

                FilteredMeds = new ListCollectionView(Meds);
                _filteredMeds.Filter = FilterMeds;
            }
            catch (Exception e)
            {
                MessageBox.Show("Lista de medicamente nu a putut fi redată!\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                isLoading = false;
            }
        
        }
    }
}
