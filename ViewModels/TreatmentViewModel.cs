using System;
using System.Collections.Generic;
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
    internal class TreatmentViewModel : ViewModelBase
    {
        private readonly int _passedId;

        private Treatment _treatment;
        public Treatment Treatment
        {
            get => _treatment;
            set
            {
                _treatment = value;
                OnPropertyChanged(nameof(Treatment));
            }
        }

        public ICommand RepeatTreatmentCommand { get; set; }

        public ICommand NavigateBackCommand { get; }


        public TreatmentViewModel(NavigationStore navigationStore, int? passedId)
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
                await LoadTreatment();
            });

            RepeatTreatmentCommand = new RelayCommand(RepeatTreatment);

            NavigateBackCommand = new NavigateCommand<TreatmentsViewModel>
              (new NavigationService<TreatmentsViewModel>(_navigationStore, (id) => new TreatmentsViewModel(_navigationStore)));
        }

        private async void RepeatTreatment(object parameter)
        {
         
            var result = Boxes.ConfirmBox("Sunteți sigur ca doriți sa repetați tratementul cu numărul: " + Treatment.Id + "?");

            if (result == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            try
            {
                Treatment newTreatment = await DuplicateObjectService.DuplicateTreatment(Treatment);

                NavigationService<TreatmentViewModel> navigationService = new NavigationService<TreatmentViewModel>
                    (_navigationStore, (id) => new TreatmentViewModel(_navigationStore, id));

                if( newTreatment != null)
                {
                    navigationService.Navigate(newTreatment.Id);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error", e.ToString());
            }
        }

        private async Task LoadTreatment()
        {
            try
            {
                Treatment = await new TreatmentRepository().GetById(_passedId);

                if( Treatment is null )
                {
                    Boxes.ErrorBox("Tratamentul nu a fost găsit!");
                    return;
                }

                _navigationStore.PageTitle = "Tratamentul cu numărul: " + Treatment.Id;

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Tratamentul nu a fost găsit!\n" + e.Message);
                Logger.LogError("Error", e.ToString());
            }
        }
    }
}
