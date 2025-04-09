using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{

    internal class RegistryRecordViewModel : ViewModelBase
    {
        private RegistryRecord _registryRecord;
        public RegistryRecord RegistryRecord
        {
            get => _registryRecord;
            set
            {
                _registryRecord = value;
                OnPropertyChanged(nameof(RegistryRecord));
            }
        }

        private readonly int? _passedId;

        public ICommand RepeatTreatmentCommand { get; set; }

        public ICommand NavigateBackCommand { get; }

        public RegistryRecordViewModel(NavigationStore navigationStore, int? passedId) 
        {
            _navigationStore = navigationStore;
            if (passedId.HasValue)
            {
                _passedId = passedId.Value;
            }
            else
            {
                _passedId = -1;
            }

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                await LoadRegistryRecord();
            });

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment);

            NavigateBackCommand = new NavigateCommand<RegistryViewModel>
               (new NavigationService<RegistryViewModel>(_navigationStore, (id) => new RegistryViewModel(_navigationStore)));
        }

        private async void RepeatTreatment(object parameter)
        {

            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + RegistryRecord.Id + "?");

            if (result == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            try
            {
                RegistryRecord newRegistryRecord = await DuplicateObjectService.DuplicateRegistryRecord(RegistryRecord);

                if (newRegistryRecord is null)
                {
                    return;
                }


                Notification Notification = new Notification()
                {
                    Type = "new-recipe",
                    Title = "A fost creată rețeta cu numărul:" + newRegistryRecord.RecipeNumber,
                    Message = "",
                    SentAt = DateTime.Now,
                    UserType = "user"
                };

                NotificationService.SendNotification(Notification);

                NavigationService<RegistryRecordViewModel> navigationService = new NavigationService<RegistryRecordViewModel>
                    (_navigationStore, (id) => new RegistryRecordViewModel(_navigationStore, id));

                navigationService.Navigate(newRegistryRecord.Id);
            }
            catch (Exception e)
            {
                Logger.LogError("Error", e.ToString());
            }
        }

        private async Task LoadRegistryRecord()
        {
            try
            {
                RegistryRecord = await new RegistryRecordRepository().GetRegistryRecordById((int)_passedId);


                if (RegistryRecord is null)
                {
                    Boxes.ErrorBox("Tratamentul nu a fost găsit!");
                    return;
                }

                _navigationStore.PageTitle = "Tratamentul cu numărul: " + RegistryRecord.Id;

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Tratamentul nu a fost găsit!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
        }
    }
}
