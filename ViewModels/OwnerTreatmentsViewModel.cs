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
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.Views;

namespace VetManagement.ViewModels
{
    public  class OwnerTreatmentsViewModel : ViewModelBase
    {

        private readonly int PassedId;

        private readonly string _pageTitle;

        public ICommand NavigateOwnersCommand { get; set; }
        public ICommand NavigateCreateTreatment { get; set; }

        public Owner? Owner;

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

            _navigationStore = navigationStore;
            _navigationStore.PassedId = PassedId;

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            NavigateCreateTreatment = new NavigateWindowCommand<CreateOwnerTreatmentViewModel>
                (new NavigationService<CreateOwnerTreatmentViewModel>(_navigationStore, (id) => new CreateOwnerTreatmentViewModel(OnTreatmentCreated, id) ), () => new CreateOwnerTreatmentWindow());
          

        }

        private void OnTreatmentCreated( Treatment treatment)
        {
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
            _navigationStore.PageTitle = "Tratamentele lui " + Owner.Name; 


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
