using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private new readonly NavigationStore _navigationStore;

        private new readonly WindowStore _windowStore;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateOwnersCommand { get; }
        public ICommand NavigateTreatmentsCommand { get; }
        public ICommand NavigateInventoryCommand { get; }
        public ICommand NavigateRegistryCommand { get; }
        public ICommand NavigateMedReportsCommand { get; }
        public ICommand NavigateAppSettingsCommand { get; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public bool IsRoot { get; } = false;
        public new string PageTitle 
        { 
            get => _navigationStore.PageTitle; 
            
            set
            {
                _navigationStore.PageTitle = value;
            }
        } 

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _navigationStore.PageTitleChanged += OnPageTitleChanged;

            if (SessionManager.Instance.Role == "admin")
            {
                IsRoot = true;
            }

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>
                (new NavigationService<HomeViewModel>(_navigationStore, (id) => new HomeViewModel(_navigationStore))); 

            NavigateUsersCommand = new NavigateCommand<UsersViewModel>
                (new NavigationService<UsersViewModel>(_navigationStore, (id) => new UsersViewModel(_navigationStore)));
            
            NavigateInventoryCommand = new NavigateCommand<InventoryViewModel>
                (new NavigationService<InventoryViewModel>(_navigationStore, (id) => new InventoryViewModel(_navigationStore)));

            NavigateTreatmentsCommand = new NavigateCommand<TreatmentsViewModel>
                (new NavigationService<TreatmentsViewModel>(_navigationStore, (id) => new TreatmentsViewModel(_navigationStore)));

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateRegistryCommand = new NavigateCommand<RegistryViewModel>
                (new NavigationService<RegistryViewModel>(_navigationStore, (id) => new RegistryViewModel(_navigationStore)));

            NavigateAppSettingsCommand = new NavigateCommand<AppSettingsViewModel>
                (new NavigationService<AppSettingsViewModel>(_navigationStore, (id) => new AppSettingsViewModel(_navigationStore)));

            NavigateMedReportsCommand = new NavigateCommand<MedReportsViewModel>
                (new NavigationService<MedReportsViewModel>(_navigationStore, (id) => new MedReportsViewModel(_navigationStore)));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnPageTitleChanged()
        {
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}
