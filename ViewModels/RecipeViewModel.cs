using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.ViewModels
{
    public class RecipeViewModel : ViewModelBase
    {
        private int _passedId;


        private Recipe _recipe;
        public Recipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                OnPropertyChanged(nameof(Recipe));
            }
        }

        public ICommand SignRecipeCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public RecipeViewModel(NavigationStore navigationStore, int? id)
        {
            _navigationStore = navigationStore;

            SignRecipeCommand = new RelayCommand(SignRecipe);

            NavigateBackCommand = new NavigateCommand<RecipeListViewModel>
                (new NavigationService<RecipeListViewModel>(_navigationStore, (id) => new RecipeListViewModel(_navigationStore)));

            if (id.HasValue)
            {
                _passedId = id.Value;
            }
            else
            {
                _passedId = -1;
            }
        }

        private async void SignRecipe( object sender)
        {
            Recipe.Signed = true;
            Recipe.OwnerSignature = "longsignatruestringrepresentingabinaryimage";

            try
            {
                await new BaseRepository<Recipe>().Update(Recipe);
                Boxes.InfoBox("Reteta a fost semnată!");

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Probleme in semnarea rețetei!\n" + e.Message);
            }
        }

        public async Task LoadRecipe()
        {
            try
            {
                Recipe recipe = await new RecipeRepository().GetById(_passedId);

                if(recipe != null)
                {
                    Recipe = recipe;
                    _navigationStore.PageTitle = "📝 Rețeta cu numărul: " + Recipe.Id;

                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Reteta nu a fost găsită\n" + e.Message);
            }
        }

    }
}
