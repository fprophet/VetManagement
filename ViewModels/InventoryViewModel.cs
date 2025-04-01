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
using VetManagement.DataWrappers;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        private string _pageTitle = "📦 Inventar";

        private new readonly NavigationStore _navigationStore;

        public CreateMedViewModel CreateMedViewModel { get;  }

        public ICommand NavigateViewMedCommand { get; }

        public ICommand DeleteMedCommand { get; }

        public ICommand EditItemCommand { get; }

        public ICommand OpenCreateMedWindowCommand { get; }

        public ObservableCollection<MedWrapper> Meds { get; set; } = new ObservableCollection<MedWrapper>();

        public ObservableCollection<object> MedTypeList { get; set; } =

           new ObservableCollection<object> { new { Name = "Medicament", Value = "medicament" }, new { Name = "Vaccin", Value = "vaccin" } };

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

        private string _typeFilter = "medicament";
        public string TypeFilter
        {
            get => _typeFilter;
            set
            {
                _typeFilter = value;
                Meds.Clear();
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(TypeFilter));
            }
        }  
        
        private string _nameFilter = "";
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                Meds.Clear();
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(NameFilter));
            }
        }

        private DateTime? _valabilityFilter = null;
        public DateTime? ValabilityFilter
        {
            get => _valabilityFilter;
            set
            {
                _valabilityFilter = value;
                Meds.Clear();
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(ValabilityFilter));
            }
        }

        private DateTime? _dateAddedFilter = null;
        public DateTime? DateAddedFilter
        {
            get => _dateAddedFilter;
            set
            {
                _dateAddedFilter = value;
                Meds.Clear();
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(DateAddedFilter));
            }
        }

        private string _lotFilter = "";
        public string LotFilter
        {
            get => _lotFilter;
            set
            {
                _lotFilter = value;
                Meds.Clear();
                isLoading = true;
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(_lotFilter));
            }
        }

        public InventoryViewModel(NavigationStore navigationStore) 
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            _filterService = new FilterService(LoadMeds);

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadMeds());

            PaginationService = new PaginationService(LoadMeds,LoadMeds,20);

            DeleteMedCommand = new RelayCommand(DeleteMed);
            EditItemCommand = new RelayCommand(UpdateMed);
            NavigateViewMedCommand = new NavigateCommand<MedViewModel>
                (new NavigationService<MedViewModel>(_navigationStore, (id) => new MedViewModel(_navigationStore,id)));
            
            OpenCreateMedWindowCommand = new NavigateWindowCommand<CreateMedViewModel>
                (new WindowService<CreateMedViewModel>(_navigationStore, (id) => new CreateMedViewModel(_navigationStore, UpdateMedList)), () => new CreateMedWindow());
        }


        private void UpdateMedList(Med med)
        {

            Meds.Add( new MedWrapper(med));


            var sorted = Meds.OrderByDescending(m => m.DateAdded).ToList();
            
            Meds.Clear();

            foreach ( var itm in sorted)
            {
                Meds.Add(itm);
            }

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

            Meds.Clear();
            try
            {

                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters["typeFilter"] = TypeFilter;
                filters["nameFilter"] = NameFilter;
                filters["lotFilter"] = LotFilter;
                filters["valabilityFilter"] = ValabilityFilter;
                filters["dateAddedFilter"] = DateAddedFilter;

                var (meds,totalRecords) = await new MedRepository().GetAllFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);

                PaginationService.TotalFound = totalRecords;

                var sorted = meds.OrderByDescending(m => m.Id).ToList();

                Meds.Clear();

                foreach (var med in sorted)
                {
                    Meds.Add( new MedWrapper(med));
                }
              
            }
           
            catch (Exception e)
            {
                MessageBox.Show("Lista de medicamente nu a putut fi redată!\n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }finally
            {
                isLoading = false;
            }


        }
    }
}
