using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class MedViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;

        private readonly int _passedId;

        private readonly string _pageTitle;

        private readonly BaseRepository<Med> _medRepository = new BaseRepository<Med>();
        public ObservableCollection<TreatmentMed> Treatments { get; set; } = new ObservableCollection<TreatmentMed>();

        private Med _med;
        public Med CurrentMed 
        { 
            get => _med;
            set
            {
                _med = value;
                OnPropertyChanged(nameof(CurrentMed));
            }
        }

        public ICommand NavigateInventoryCommand { get; }


        private string _name;

        public string Name 
        {   get => _name;
            set 
            { 
                _name = value;
                OnPropertyChanged(nameof(Name));
            } 
        }

        public MedViewModel(NavigationStore navigationStore,int? id)
        {
            _navigationStore = navigationStore;

            NavigateInventoryCommand = new NavigateCommand<InventoryViewModel>
                (new NavigationService<InventoryViewModel>(navigationStore, (id) => new InventoryViewModel(navigationStore)));
            if (id.HasValue)
            {
                _passedId = id.Value;
            }
            else
            {
                _passedId = -1;
            }
            LoadMed();

            LoadMedTreatments();

        }

        private async void LoadMedTreatments()
        {
            TreatmentMedRepository treatmentMedRepository = new();

            var treatments = await treatmentMedRepository.GetTreatmentsForMed(_passedId);

            var sortedTreatments = treatments.OrderByDescending(t => t.Treatment.DateAdded);

            foreach( var treatment in sortedTreatments)
            {
                Treatments.Add(treatment);

            }
        }

        private async void LoadMed()
        {
            try 
            {
                _med =  await _medRepository.GetById(_passedId);
                _navigationStore.PageTitle = "Tratamente cu: " + _med.Name;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Medicamentul nu a putut fi găsit!\n" + ex.Message, "Error", MessageBoxButton.OK);

            }


        }
    }
}
