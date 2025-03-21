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
using VetManagement.Commands;
using VetManagement.Data;
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

        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "admin", "user" };

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

        public UsersViewModel(NavigationStore navigationStore)
        {

            Users  = new ObservableCollection<User>();
         
            _navigationStore = navigationStore;
            _userRepository = new BaseRepository<User>();
            _navigationStore.PageTitle = "🔐 Utilizatori înregistrați";

            OnLoadedCommand = new RelayCommand(async (object parameter) => await LoadUsers());

            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            EditUserCommand = new RelayCommand(EditUser);

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>
                (new NavigationService<HomeViewModel>(_navigationStore,(id) => new HomeViewModel(_navigationStore)));

            NavigateCreateUserWindow = new NavigateWindowCommand<CreateUserViewModel>
                (new WindowService<CreateUserViewModel>(_navigationStore, (id) => new CreateUserViewModel(_navigationStore,OnUserCreated)), () => new CreateUserWindow());

            SendNotificationCommand = new RelayCommand(SendNot);
        }

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

            if (parameter is int userId)
            {
                var result = MessageBox.Show($"Sunteti sigur ca doriți să ștergeți acest utilizator?",
                                             "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _userRepository.Delete(userId);
                        Users.Remove(Users.First(u => u.Id == userId));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Utilizatorul nu a putut fi șters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError("Database", e.Message);
                    }

                }
            }
        }

        public async Task LoadUsers()
        {
            try
            {
                var users = await _userRepository.GetAll();

                Users.Clear();

                foreach (var user in users)
                {
                    Users.Add(user);
                }

          
            }
            catch (Exception e) 
            {
                MessageBox.Show("Lista de utilizatori este indisponibilă!\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        


    }
}
