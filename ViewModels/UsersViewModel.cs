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

        private readonly NavigationStore _navigationStore;

        public ObservableCollection<User> Users { get; private set; } = new ObservableCollection<User>();

        public ObservableCollection<string> Roles { get; private set; } = new ObservableCollection<string> { "admin", "user" };

        public ICommand NavigateHomeCommand { get; }

        public ICommand NavigateViewMedCommand { get; }

        public ICommand DeleteUserCommand { get; }

        public ICommand ToggleFormVisibilityCommand { get; }

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
            _navigationStore = navigationStore;
            _userRepository = new BaseRepository<User>();

            CreateUserViewModel = new CreateUserViewModel(OnUserCreated);

   
            LoadUsers();
            ToggleFormVisibilityCommand = new RelayCommand(ToggleFormVisibility);
            DeleteUserCommand = new RelayCommand(DeleteUser);

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore,(id) => new HomeViewModel(_navigationStore)));

        }

        private void ToggleFormVisibility(object parameter) {

            isVisibleForm = !isVisibleForm;

        }

        private void OnUserCreated(User newUser)
        {
            Users.Add(newUser);
        }

        private async void DeleteUser(object parameter) 
        {

            if (parameter is User userToDelete)
            {
                var result = MessageBox.Show($"Sunteti sigur ca doriți să stergeti utilizatorul {userToDelete.Name}?",
                                             "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _userRepository.Delete(userToDelete.Id);
                        Users.Remove(userToDelete);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Utilizatorul nu a putut fi șters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        Logger.LogError("Database", e.Message);
                    }

                }
            }
        }

        private async void LoadUsers()
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
