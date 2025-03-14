using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private new readonly NavigationStore _navigationStore;


        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateOwnersCommand { get; }
        public ICommand NavigateTreatmentsCommand { get; }
        public ICommand NavigateInventoryCommand { get; }
        public ICommand NavigateRegistryCommand { get; }
        public ICommand NavigateMedReportsCommand { get; }
        public ICommand NavigateAppSettingsCommand { get; }
        public ICommand NavigateRecipeListCommand { get; }
        public ICommand NavigateNotificationCommand { get; }
        public ICommand NavigateImportedProductsCommand { get; }
        public ICommand ToggleSideMenuCommand { get; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public ObservableCollection<Notification> Notifications { get; } = new ObservableCollection<Notification>();

        public bool IsRoot { get; } = false;
        public new string PageTitle 
        { 
            get => _navigationStore.PageTitle; 
            
            set
            {
                _navigationStore.PageTitle = value;
            }
        }

        public bool HasNotifications
        { 
            get => Notifications.Count() > 0; 

        }

        public int NotificationCount
        { 
            get => Notifications.Count(); 
            
        }


        private string _messageHolder;
        public string MessageHolder
        {
            get => _messageHolder;
            set
            {
                _messageHolder = value;
                OnPropertyChanged(nameof(MessageHolder));
            }
        }

        private int _sideMenuWidth = 200;
        public int SideMenuWidth
        {
            get => _sideMenuWidth;
            set
            {
                _sideMenuWidth = value;
                OnPropertyChanged(nameof(SideMenuWidth));
            }
        }  
        
        private string _toggleButtonSymbol = "⬅️";
        public string ToggleButtonSymbol
        {
            get => _toggleButtonSymbol;
            set
            {
                _toggleButtonSymbol = value;
                OnPropertyChanged(nameof(ToggleButtonSymbol));
            }
        }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _navigationStore.PageTitleChanged += OnPageTitleChanged;

            Notifications.CollectionChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(NotificationCount));
                OnPropertyChanged(nameof(HasNotifications));
                
            };

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

            NavigateRecipeListCommand = new NavigateCommand<RecipeListViewModel>
                (new NavigationService<RecipeListViewModel>(_navigationStore, (id) => new RecipeListViewModel(_navigationStore)));

            NavigateImportedProductsCommand = new NavigateCommand<ImportedProductsViewModel>
                (new NavigationService<ImportedProductsViewModel>(_navigationStore, (id) => new ImportedProductsViewModel(_navigationStore)));

            NavigateNotificationCommand = new RelayCommand(NavigateNotification);
            ToggleSideMenuCommand = new RelayCommand(ToggleSideMenu);

        }

        private void ToggleSideMenu(object parameter)
        {
            if (SideMenuWidth > 0)
            {
                SideMenuWidth = 0;
                ToggleButtonSymbol = "➡️";
            }
            else
            {
                ToggleButtonSymbol = "⬅️";
                SideMenuWidth = 200;
            }
        }

        private void NavigateNotification(object parameters)
        {
            

            var values = (object[])parameters;

            var type = (string)values[0];
            var title = (string)values[1];
           
            if( type == "new-recipe")
            {
                NavigationService<RecipeViewModel> NavService = new NavigationService<RecipeViewModel>(_navigationStore, (id) => new RecipeViewModel(_navigationStore,id));

                int id = NotificationService.RecipeIdFromTitle(title);
                if (id > 0)
                {
                    NavService.Navigate(id);

                    SetNotificationAsRead(title);
                }
                else 
                {
                    SetNotificationAsRead(title);
                    Boxes.InfoBox("Eroare în navigare!");
                }
            }

            //SetNotificationAsRead(title);


        }

        private void SetNotificationAsRead(string title)
        {
            Notification notification = Notifications.FirstOrDefault(n => n.Title == title);

            notification.Read = true;
            notification.ReadAt = DateTime.Now;

            new BaseRepository<Notification>().Update(notification);

            Notifications.Remove(notification);

        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnPageTitleChanged()
        {
            OnPropertyChanged(nameof(PageTitle));
        }

        private async void UpdateNotifications(string message)
        {
            Notification notification = await NotificationService.GetLastNotification();

            if( notification == null)
            {
                return;
            }

            if( notification.UserType != SessionManager.Instance.Role)
            {
                return;
            }

            Notifications.Add(notification);
        }

        public async Task ListenForNotifications()
        {
            NotificationService notificationService = new NotificationService(UpdateNotifications);

            await notificationService.StartListening();
        }

        public async Task LoadNotifications()
        {
  
            try
            {
                var notifications = await new NotificationRepository().GetForUserType(SessionManager.Instance.Role);

                var sorted = notifications.OrderByDescending(n => n.SentAt);

                foreach( Notification notification in sorted)
                {
                    Notifications.Add(notification);
                }

            }
            catch(Exception e)
            {
                Logger.LogError("Errors", e.ToString());
            }
        }
    }
}
