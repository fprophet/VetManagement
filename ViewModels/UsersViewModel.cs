using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly BaseRepository<User> _userRepository;

        private new readonly NavigationStore _navigationStore;

        public ObservableCollection<User> Users { get; set; }

        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "manager", "faramcist", "asistent" };

        public ICommand NavigateHomeCommand { get; }

        public ICommand NavigateViewMedCommand { get; }

        public ICommand DeleteUserCommand { get; }

        public ICommand EditUserCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

        public ICommand NavigateCreateUserWindow { get; }

        public ICommand SendNotificationCommand { get; }

        public CreateUserViewModel CreateUserViewModel { get; }

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

        private object _selectedRow;
        public object SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (value is User)
                {
                    SelectedUser = (User)value;
                }

                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public UsersViewModel(NavigationStore navigationStore)
        {

            Users  = new ObservableCollection<User>();
         
            _navigationStore = navigationStore;
            _userRepository = new BaseRepository<User>();
            _navigationStore.PageTitle = "🔐 Utilizatori înregistrați";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadUsers());

            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanExecuteUserAction);
            EditUserCommand = new RelayCommand(EditUser);

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>
                (new NavigationService<HomeViewModel>(_navigationStore,(id) => new HomeViewModel(_navigationStore)));

            NavigateCreateUserWindow = new NavigateWindowCommand<CreateUserViewModel>
                (new WindowService<CreateUserViewModel>(_navigationStore, (id) => new CreateUserViewModel(_navigationStore,OnUserCreated)), () => new CreateUserWindow());

            SendNotificationCommand = new RelayCommand(SendNot);

        }

        public bool CanExecuteUserAction(object parameter) => _selectedUser != null && _selectedUser.Id is int;

        private void SendNot(object parameter)
        {

            Notification Notification = new Notification()
            {
                Type = "test-type",
                Title = "test-title",
                Message = "test-message",
                SentAt = DateTime.Now,
                UserType = "test-user-type"
            };

            NotificationService.SendNotification(Notification);
        }

        private void ToggleFormVisibility(object parameter) {

            isVisibleForm = !isVisibleForm;

        }

        private void OnUserCreated(User newUser)
        {
            Users.Add(newUser);
        }

        private async void EditUser(object parameter)
        {
            try
            {
                if (parameter is User toUpdate)
                {
                    await new BaseRepository<User>().Update(toUpdate);
                    Boxes.InfoBox("Utilizatorul a fost acutalizat!");
                }
            }catch(Exception e)
            {
                Boxes.ErrorBox("Utilizatorul nu a putut fi actualizat!\n" + e.Message);
            }
           
        }

        private async void DeleteUser(object parameter) 
        {
            var result = MessageBox.Show($"Sunteti sigur ca doriți să ștergeți acest utilizator?",
                                         "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            if (_selectedUser == null)
            {
                return;
            }
            try
            {
                await _userRepository.Delete(_selectedUser.Id);
                Users.Remove(Users.First(u => u.Id == _selectedUser.Id));
            }
            catch (Exception ex)
            {
                Logger.LogError("Database", ex.ToString());
                Boxes.ErrorBox("Utilizatorul nu a putut fi șters!\n" + ex.Message);
            }
                
        }

        public async Task LoadUsers()
        {
            Users.Clear();
            try
            {
                await Task.Run(async () =>
                {
                    var users = await _userRepository.GetAll();

                    foreach (var user in users)
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Users.Add(user);
                        }, DispatcherPriority.Background);
                    }
                });
            }
            catch (Exception ex) 
            {
                Logger.LogError("Error", ex.ToString());
                Boxes.ErrorBox("Lista de utilizatori este indisponibilă!\n" + ex.Message);

            }
        }

        


    }
}
