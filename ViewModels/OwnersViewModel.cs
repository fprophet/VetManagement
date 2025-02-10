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
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class OwnersViewModel : ViewModelBase
    {

        private string _pageTitle = "Pagină proprietari";

        public ICommand NavigateCreateOwnerCommand { get; set; }

        public ICommand DeleteOwnerCommand { get; set; }

        public ICommand NavigateCreatePatientCommand { get; set; }

        public ICommand NavigateCreateTreatmentCommand { get; set; }

        private OwnerRepository _ownerRepository;

        public ObservableCollection<Owner> Owners { get; set; } = new ObservableCollection<Owner>();

        public OwnersViewModel(NavigationStore navigationStore)
        {
            _ownerRepository = new OwnerRepository();
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = _pageTitle;

            NavigateCreateOwnerCommand = new NavigateCommand<CreateOwnerViewModel>
                (new NavigationService<CreateOwnerViewModel>(_navigationStore, (id) => new CreateOwnerViewModel(_navigationStore)));
            NavigateCreatePatientCommand = new NavigateCommand<CreatePatientViewModel>
                (new NavigationService<CreatePatientViewModel>(_navigationStore, (id) => new CreatePatientViewModel(_navigationStore,id)));
            NavigateCreateTreatmentCommand = new NavigateCommand<OwnerTreatmentsViewModel>
                (new NavigationService<OwnerTreatmentsViewModel>(_navigationStore, (id) => new OwnerTreatmentsViewModel(_navigationStore, id)));

            DeleteOwnerCommand = new RelayCommand(DeleteOwner);

            LoadOwners();
        }

        private async void LoadOwners()
        {
            var owners = await _ownerRepository.GetFullInfo();

            var sortedOwners = owners.OrderByDescending(o => o.DateAdded);

            foreach ( var owner in sortedOwners)
            {
                Owners.Add(owner);

            }

        }

        private async void DeleteOwner(object parameter)
        {
            try 
            {
                var result = Boxes.ConfirmBox("Doriți să ștergeți proprietarul?");
                if (parameter is int && result == MessageBoxResult.Yes) 
                {
                    await _ownerRepository.Delete((int)parameter);

                    var owner = Owners.FirstOrDefault(o => o.Id == (int)parameter);

                    Owners.Remove(owner);
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
        }
    }
}
