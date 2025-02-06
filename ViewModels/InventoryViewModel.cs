using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Google.Protobuf.WellKnownTypes;
using Mysqlx;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        private readonly BaseRepository<Med> _medRepository;

        private bool _isVisibleForm = false;

        public bool isVisibleForm
        {
            get => _isVisibleForm;
            set
            {
                _isVisibleForm = value;
                OnPropertyChanged(nameof(isVisibleForm));
            }
        }

        public CreateMedViewModel CreateMedViewModel { get;  }

        public ICommand NavigateHomeCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        public ICommand NavigateViewMedCommand { get; }

        public ICommand DeleteMedCommand { get; }


        public ObservableCollection<Med> Meds { get; set; } = new ObservableCollection<Med>();

        public InventoryViewModel(NavigationStore navigationStore) 
        { 
            _navigationStore = navigationStore;
            _medRepository = new BaseRepository<Med>();

            CreateMedViewModel = new CreateMedViewModel(UpdateMedList);

            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);
            DeleteMedCommand = new RelayCommand(DeleteMed);
            NavigateViewMedCommand = new NavigateCommand<MedViewModel>(new NavigationService<MedViewModel>(_navigationStore, (id) => new MedViewModel(_navigationStore, id)));


            LoadMeds();

        }

        private void UpdateMedList(Med med)
        { 
            Meds.Add(med);
        }

        private void ToggleFormVisibility(object parameter)
        {
            isVisibleForm = !isVisibleForm;
        }

        private async void  DeleteMed(object parameter)
        {
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa ștergeți acest medicament?");

            if (parameter is int && result == MessageBoxResult.Yes)
            {
                try
                {
                    await _medRepository.Delete((int)parameter);
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

        private async void LoadMeds() 
        {
            try
            {
                var meds = await _medRepository.GetAll();

                Meds.Clear();

                foreach (var med in meds)
                {
                    Meds.Add(med);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Lista de medicamente nu a putut fi redată!\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        }
    }
}
