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
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _ptVisibility;
        private bool _ltVisibility;
        private bool _srVisibility;
        private bool _nmVisibility;

        public DateTime CurrentDate { get; } = DateTime.Now.Date;
        public ObservableCollection<Treatment> PetTreatments { get; } = new ObservableCollection<Treatment>();
        public ObservableCollection<RegistryRecord> LivestockTreatments { get; } = new ObservableCollection<RegistryRecord>();
        public ObservableCollection<Recipe> SignedRecipes { get; } = new ObservableCollection<Recipe>();
        public ObservableCollection<Med> NewMeds { get; } = new ObservableCollection<Med>();
        public ObservableCollection<TreatmentMed> UsedMeds { get; } = new ObservableCollection<TreatmentMed>();
        public ObservableCollection<Treatment> AllTreatments { get; set; }

        public ICommand ToggleGridCommand { get; }
        public ICommand NavigatePetTreatmentCommand { get; }
        public ICommand NavigateRecipeViewCommand { get; }
        public ICommand NavigateMedViewCommand { get; }
        public ICommand NavigateLivestockTreatmentCommand { get; }

        public bool PTVisibility
        {
            get => _ptVisibility;
            set
            {
                _ptVisibility = value;
                OnPropertyChanged(nameof(PTVisibility));
            }
        }

        public bool LTVisibility
        {
            get => _ltVisibility;
            set
            {
                _ltVisibility = value;
                OnPropertyChanged(nameof(LTVisibility));
            }
        }

        public bool SRVisibility
        {
            get => _srVisibility;
            set
            {
                _srVisibility = value;
                OnPropertyChanged(nameof(SRVisibility));
            }
        }

        public bool NMVisibility
        {
            get => _nmVisibility;
            set
            {
                _nmVisibility = value;
                OnPropertyChanged(nameof(NMVisibility));
            }
        }



        public HomeViewModel( NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PageTitle = "🏠 Acasă " + CurrentDate.ToString("yyyy-MM-dd");

            ToggleGridCommand = new RelayCommand(ToggleGrid);

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                try
                {
                    //await GetTodaysPetTreatments();
                    //await GetTodaysLivestockTreatments();
                    //await GetTodaysSignedRecipes();
                    //await GetTodaysNewMeds();
                    //await GetTodaysUsedMeds();

                }
                catch(Exception e)
                {
                    Boxes.ErrorBox("Date nu au putut fi redate!");
                    Logger.LogError("Error", e.ToString());
                }
                
            });

            NavigatePetTreatmentCommand = new NavigateCommand<TreatmentViewModel>
                (new NavigationService<TreatmentViewModel>(_navigationStore, (id) => new TreatmentViewModel(_navigationStore, id)));

            NavigateLivestockTreatmentCommand = new NavigateCommand<RegistryRecordViewModel>
                (new NavigationService<RegistryRecordViewModel>(_navigationStore, (id) => new RegistryRecordViewModel(_navigationStore, id)));

            NavigateRecipeViewCommand = new NavigateCommand<RecipeViewModel>
                (new NavigationService<RecipeViewModel>(_navigationStore, (id) => new RecipeViewModel(_navigationStore, id)));
            
            NavigateMedViewCommand = new NavigateCommand<MedViewModel>
                (new NavigationService<MedViewModel>(_navigationStore, (id) => new MedViewModel(_navigationStore, id)));
        }

        private void ToggleGrid(object parameter)
        {
            string gridName = (string)parameter;
            if( string.IsNullOrEmpty(gridName))
            {
                return;
            }

            switch(gridName)
            {
                case "PT":
                    PTVisibility = !PTVisibility;
                    break;
                case "LT":
                    LTVisibility = !LTVisibility;
                    break;
                case "SR":
                    SRVisibility = !SRVisibility;
                    break; 
                case "NM":
                    NMVisibility = !NMVisibility;
                    break;
            }
        }

        private async Task GetTodaysPetTreatments()
        {
            var treatments = await new TreatmentRepository().GetTodaysTreatments();
            foreach( Treatment treatment in treatments)
            {
                 PetTreatments.Add(treatment);
            }

        }
         private async Task GetTodaysLivestockTreatments()
        {
            var treatments = await new RegistryRecordRepository().GetTodaysLivestockTreatments();
            foreach(RegistryRecord treatment in treatments)
            {
                LivestockTreatments.Add(treatment);
            }

        }



        private async Task GetTodaysSignedRecipes()
        {
            var recipes = await new RecipeRepository().GetTodaysSignedRecipes();
            foreach (Recipe recipe in recipes)
            {
                SignedRecipes.Add(recipe);
            }
        }
        
        private async Task GetTodaysNewMeds()
        {
            var meds = await new MedRepository().GetTodaysNewMeds();
            foreach (Med med in meds)
            {
                NewMeds.Add(med);
            }
        }
        
        private async Task GetTodaysUsedMeds()
        {
            //AllTreatments = new ObservableCollection<Treatment>(PetTreatments.Concat(LivestockTreatments));

            //UniqueMedsTreatments = AllTreatments.DistinctBy("t") 
        }


    }
}
