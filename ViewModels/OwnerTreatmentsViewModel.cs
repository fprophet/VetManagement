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

namespace VetManagement.ViewModels
{
    public  class OwnerTreatmentsViewModel : ViewModelBase
    {

        private readonly int PassedId;

        private readonly string _pageTitle;


        public ICommand NavigateOwnersCommand { get; set; }

        public Owner Owner;

        private TreatmentRepository _treatmentRepository;

        public CreateTreatmentViewModel CreateTreatmentViewModel { get; set; }

        public ObservableCollection<Treatment> Treatments { get; private set; } = new ObservableCollection<Treatment>();


        public OwnerTreatmentsViewModel( NavigationStore navigationStore, int? id) 
        { 
            _navigationStore = navigationStore;
            _treatmentRepository = new TreatmentRepository();

            NavigateOwnersCommand = new NavigateCommand<OwnersViewModel>
                (new NavigationService<OwnersViewModel>(_navigationStore, (id) => new OwnersViewModel(_navigationStore)));

            if (id.HasValue)
            {
                PassedId = id.Value;
            }
            else
            {
                PassedId = -1;
            }

            LoadTreatments();
            LoadOwner();

            CreateTreatmentViewModel = new CreateTreatmentViewModel(OnTreatmentCreated, id);

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


        private async void LoadOwner()
        {
            BaseRepository<Owner> ownerRepository = new BaseRepository<Owner>();
            Owner = await ownerRepository.GetById(PassedId);
            _navigationStore.PageTitle = "Tratamentele lui " + Owner.Name; 


        }

        private async void LoadTreatments()
        {
            var treatments = await _treatmentRepository.GetFullTreatmentsForOwner(PassedId);

            var sortedTreatments = treatments.OrderByDescending(t => t.DateAdded);

            foreach (var treatment in sortedTreatments)
            {
               
                Treatments.Add(treatment);
            }


        }
    }
}
