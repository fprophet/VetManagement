using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
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
                if (value is Treatment)
                {
                    SelectedTreatment = (Treatment)value;
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

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();


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

            _navigationStore = navigationStore;
            _navigationStore.PassedId = PassedId;

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment, CanExecuteMedAction);

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateCreateTreatment = new NavigateWindowCommand<CreateTreatmentViewModel>
                (new WindowService<CreateTreatmentViewModel>(_navigationStore, (id) => new CreateTreatmentViewModel(_navigationStore,OnTreatmentCreated, id, "pet") ), () => new CreateOwnerTreatmentWindow());

        }

        public bool CanExecuteMedAction(object parameter) => _selectedTreatment != null && _selectedTreatment.Id is int;

        private async void RepeatTreatment(object parameter)
        {
            if (_selectedTreatment.Id == null)
            {
                Boxes.InfoBox("Tratamentul nu a putut fi repetat!");
                return;
            }

            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + _selectedTreatment.Id + "?");

            if (result == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            Treatment? treatment = Treatments.FirstOrDefault(t => t.Id == _selectedTreatment.Id);

            try
            {
                Treatment newTreatment = await DuplicateObjectService.DuplicateTreatment(treatment);
                OnTreatmentCreated(newTreatment);
            }
            catch (Exception e)
            {
                Boxes.InfoBox("Tratamentul nu a putut fi repetat!\n" + e.ToString());

                Logger.LogError("Error", e.ToString());
            }
        }

        private void OnTreatmentCreated( Treatment treatment)
        {
            if (treatment is null)
            {
                return;
            }
                Treatments.Add(treatment);
            var sortedTreatments = Treatments.OrderByDescending(t => t.DateAdded).ToList();

            Treatments.Clear();

            foreach (var sorted in sortedTreatments)
            {
                Treatments.Add(sorted);
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
            var treatments = await new TreatmentRepository().GetFullTreatmentsForOwner(PassedId);

            var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);

            foreach (var treatment in sortedTreatments)
            {
                Treatments.Add(treatment);
            }


        }
    }
}
