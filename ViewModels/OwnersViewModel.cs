
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Repositories;
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
        public FilterHelper FilterHelper { get; set; } = new FilterHelper();


        private object _selectedRow;

        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if( value is Owner)
                {
                    SelectedOwner = (Owner)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private Owner _selectedOwner;
        public Owner SelectedOwner
        {
            get => _selectedOwner;
            set
            {
                _selectedOwner = value;
                OnPropertyChanged(nameof(SelectedOwner));
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

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                await LoadOwners();
            });

            _filterService = new FilterService(LoadOwners);

            PaginationService = new PaginationService(LoadOwners, LoadOwners,20);

            NavigateCreateOwnerWindowCommand = new NavigateWindowCommand<CreateOwnerAndPatientViewModel>    
                (new WindowService<CreateOwnerAndPatientViewModel>(_navigationStore, (id) => new CreateOwnerAndPatientViewModel(_navigationStore, UpdateOwners)), () => new CreateOwnerAndPatientWindow());

            NavigateTreatmentsListCommand = new NavigateCommand<OwnerTreatmentsViewModel>
                (new NavigationService<OwnerTreatmentsViewModel>(_navigationStore, (id) => new OwnerTreatmentsViewModel(_navigationStore, _selectedOwner.Id)), CanExecuteUserAction);

            NavigatePatientsListCommand = new NavigateCommand<OwnerPatientsViewModel>
                (new NavigationService<OwnerPatientsViewModel>(_navigationStore, (id) => new OwnerPatientsViewModel(_navigationStore, _selectedOwner.Id)), CanExecuteUserAction) ;

            DeleteOwnerCommand = new RelayCommand(DeleteOwner, CanExecuteUserAction);
            UpdateOwnerCommand = new RelayCommand(UpdateOwner, CanExecuteUserAction);

        }

        public bool CanExecuteUserAction(object parameter) => _selectedOwner != null && _selectedOwner.Id is int;

        public async Task LoadOwners()
        {
            Owners.Clear();
            try
            {
                Dictionary<string, string> filters = new Dictionary<string, string>();

                filters["nameFilter"] = NameFilter;
                filters["patientNameFilter"] = PatientNameFilter;
                filters["detailsFIlter"] = DetailsFilter;

                await Task.Run(async () =>
                {
                    var (owners, totalFound) = await new OwnerRepository().GetFullInfoFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PaginationService.TotalFound = totalFound;

                    });

                    foreach (var owner in owners)
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Owners.Add(owner);

                        }, DispatcherPriority.Background);
                    }

                });

                //var owners = await new OwnerRepository().GetFullInfo();



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
            if (_selectedOwner == null)
            {
                return;
            }
            try
            {
                await new OwnerRepository().Update(_selectedOwner);
                Boxes.InfoBox("Proprietarul a fost actualizat!");

            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Proprietarul nu a putut fi actualizat!\n" + e.Message);
            }
        }

        private async void DeleteOwner(object parameter)
        {
            if(_selectedOwner == null)
            {
                return;
            }
            try 
            {
                isLoading = true;
                var result = Boxes.ConfirmBox("Doriți să ștergeți proprietarul?\n Tratamentele acestuia vor fi arhivate!");

                if(result == MessageBoxResult.No)
                {
                    return;
                }

                if (_selectedOwner.Id is int ) 
                {
                    await Task.Run(async () =>
                    {
                        await OwnerService.Delete(_selectedOwner);
                    });

                    Owners.Remove(_selectedOwner);
                    _selectedOwner = null;
                    Boxes.InfoBox("Proprietarul a fost șters cu succes!");
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
            finally
            {
                isLoading = false;
            }
        }
    }
}
