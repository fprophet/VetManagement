using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Repositories;
using VetManagement.Services;
using VetManagement.Stores;

namespace VetManagement.ViewModels
{
    public class RecipeListViewModel : ViewModelBase
    {

        public ICommand NavigateRecipeCommand { get; set; }
        
        public ObservableCollection<Recipe> Recipes { get; } = new ObservableCollection<Recipe>();

        public PaginationService PaginationService { get; set; }

        private readonly FilterService _filterService;

        private bool _isLoading = false;
        public bool isLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(isLoading));
            }
        }

        private string _ownerNameFilter = "";
        public string OwnerNameFilter
        {
            get => _ownerNameFilter;
            set
            {
                _ownerNameFilter = value;
                isLoading = false;
                Recipes.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(OwnerNameFilter));
            }
        }

        private string _recipeNumberFilter ;
        public string RecipeNumberFilter
        {
            get => _recipeNumberFilter;
            set
            {
                _recipeNumberFilter = value;
                isLoading = false;
                Recipes.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(RecipeNumberFilter));
            }
        }

        private bool _onlyUsignedFilter = true;
        public bool OnlyUnsingedFilter
        {
            get => _onlyUsignedFilter;
            set
            {
                _onlyUsignedFilter = value;
                isLoading = false;
                Recipes.Clear();
                PaginationService.PageNumber = 1;
                _filterService.DebouncePropertyChanged(nameof(OnlyUnsingedFilter));
            }
        }

        public RecipeListViewModel(NavigationStore navigtionStore)
        {
            _navigationStore = navigtionStore;
            _navigationStore.PageTitle = "📝 Rețete";

            OnLoadedCommand = new RelayCommand(async (object parameter) =>
            {
                await LoadRecipes();

            });

            _filterService = new FilterService(LoadRecipes);

            PaginationService = new PaginationService(LoadRecipes, LoadRecipes,20);

            NavigateRecipeCommand =  new NavigateCommand<RecipeViewModel>
                (new NavigationService<RecipeViewModel>(_navigationStore, (id) => new RecipeViewModel(_navigationStore, id)));

        }

        public async Task LoadRecipes()
        {
            isLoading = true;
            Recipes.Clear();
            try
            {
                Dictionary<string, object> filters = new Dictionary<string, object>();

                filters["ownerNameFilter"] = OwnerNameFilter;
                filters["recipeNumberFilter"] = RecipeNumberFilter;
                filters["onlyUnsignedFilter"] = OnlyUnsingedFilter;

                await Task.Run(async () =>
                {
                    var (recipes, totalRecords) = await new RecipeRepository().GetAllFiltered(PaginationService.PageNumber, PaginationService.PerPage, filters);

                    Application.Current.Dispatcher.BeginInvoke(() =>
                    {
                        PaginationService.TotalFound = totalRecords;
                    }, DispatcherPriority.Background);

                    foreach (Recipe recipe in recipes)
                    {
                        Application.Current.Dispatcher.BeginInvoke(() =>
                        {
                            Recipes.Add(recipe);
                        }, DispatcherPriority.Background);
                    }
                });

            }
            catch(Exception e)
            {
                Boxes.ErrorBox("Rețetele nu au putut fi redate!\n" + e.Message);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
