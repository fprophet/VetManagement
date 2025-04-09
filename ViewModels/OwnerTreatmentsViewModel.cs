using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Org.BouncyCastle.Asn1.X509;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public  class OwnerTreatmentsViewModel : ViewModelBase
    {

        private readonly int PassedId;
        public ICommand NavigateOwnersCommand { get; set; }
        public ICommand NavigateCreateTreatment { get; set; }
        public ICommand RepeatTreatmentCommand { get; set; }

        public Owner? Owner;

        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is TreatmentWrapper)
                {
                    SelectedTreatment = ((TreatmentWrapper)value).Treatment;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private Treatment _selectedTreatment;
        public Treatment SelectedTreatment
        {
            get => _selectedTreatment;
            set
            {
                _selectedTreatment = value;
                OnPropertyChanged(nameof(SelectedTreatment));
            }
        }

        private bool _isLoading = false;
        public bool isLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }

        private string _treatmentTypeFilter =  SessionManager.Instance.Role == "farmacist" ?  "pet" : "";
        public string TreatmentTypeFilter
        {
            get => _treatmentTypeFilter;
            set
            {
                _treatmentTypeFilter = value;

                _filterService.DebouncePropertyChanged(nameof(TreatmentTypeFilter));

            }
        }

        public ObservableCollection<object> TreatmentTypeList { get; set; } =
            new ObservableCollection<object> { new { Name = "Toate", Value = "" }, new { Name = "Animale mici", Value = "pet" }, new { Name = "Animale mari", Value = "livestock" } };

        private readonly FilterService _filterService;

        public ObservableCollection<TreatmentWrapper> TreatmentWrappers { get; private set; } = new ObservableCollection<TreatmentWrapper>();


        public OwnerTreatmentsViewModel( NavigationStore navigationStore, int? id) 
        {

            if (id.HasValue)
            {
                PassedId = id.Value;
            }
            else
            {
                PassedId = -1;
            }

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                await LoadOwner();
                await LoadTreatments();
            });

            _filterService = new FilterService(LoadTreatments);


            _navigationStore = navigationStore;
            _navigationStore.PassedId = PassedId;

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment, CanExecuteTreatmentAction);

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateCreateTreatment = new NavigateWindowCommand<CreateTreatmentViewModel>
                (new WindowService<CreateTreatmentViewModel>(_navigationStore, (id) => new CreateTreatmentViewModel(_navigationStore,OnTreatmentCreated, id, "pet") ), () => new CreateOwnerTreatmentWindow());

        }

        public bool CanExecuteTreatmentAction(object parameter) => _selectedTreatment != null && _selectedTreatment.Id is int;

        private async void RepeatTreatment(object parameter)
        {
            
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + _selectedTreatment.Id + "?");

            if (result == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            TreatmentWrapper treatmentWrapper = TreatmentWrappers.FirstOrDefault(t => t.Treatment.Id == _selectedTreatment.Id);
            Treatment? treatment = treatmentWrapper.Treatment;

            try
            {
                Treatment newTreatment = await DuplicateObjectService.DuplicateTreatment(treatment);
                OnTreatmentCreated(newTreatment);
            }
            catch (Exception e)
            {
                Boxes.InfoBox("Tratamentul nu a putut fi repetat!\n" + e.Message);
            }
        }

        private void OnTreatmentCreated( Treatment treatment)
        {
            if (treatment is null)
            {
                return;
            }
            TreatmentWrappers.Add( new TreatmentWrapper(treatment));
            var sortedTreatments = TreatmentWrappers.OrderByDescending(t => t.DateAdded).ToList();

            TreatmentWrappers.Clear();

            foreach (var sorted in sortedTreatments)
            {
                TreatmentWrappers.Add(sorted);
            }
        }


        public async Task LoadOwner()
        {
            Owner = await new BaseRepository<Owner>().GetById(PassedId);
            if (_navigationStore is not null) 
            { 
                _navigationStore.PageTitle = "💊 Tratamentele lui " + Owner.Name;
            }
        }

        public async Task LoadTreatments()
        {
            isLoading = true;
            TreatmentWrappers.Clear();

            try
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters.Add("patientType", TreatmentTypeFilter);
                filters.Add("ownerName", Owner.Name);
           
                await Task.Run(async () =>
                {
                    var (treatments, totalFound) = await new TreatmentRepository().GetFullTreatments(1, -1, filters);
                    foreach (var treatment in treatments)

                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            TreatmentWrappers.Add(new TreatmentWrapper(treatment));

                        }, DispatcherPriority.Background);
                    }
                });
            }
            catch(Exception e)
            {
                Logger.LogError("Error", e.ToString());
                Boxes.ErrorBox("Tratamentele nu au putut fi găsite!\n" + e.Message);
            }
            finally
            {
                isLoading = false;
            }

        }
    }
}
