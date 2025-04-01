using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using VetManagement.Commands;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Migrations;
using VetManagement.Services;
using VetManagement.Stores;
using VetManagement.ViewModels;

namespace VetManagement.ViewModels
{
    public class RecipeViewModel : ViewModelBase
    {
        private int _passedId;


        private RecipeWrapper _recipeWrapper;
        public RecipeWrapper RecipeWrapper
        {
            get => _recipeWrapper;
            set
            {
                _recipeWrapper = value;
                OnPropertyChanged(nameof(RecipeWrapper));
                CommandManager.InvalidateRequerySuggested();
            }
        }


        public Action<string?>? OnRecipeSigned;


        public ICommand SignRecipeCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand PrintRecipeCommand { get; }

        public RecipeViewModel(NavigationStore navigationStore, int? id)
        {
            OnLoadedCommand = new RelayCommand(OnLoad);

            _navigationStore = navigationStore;

            SignRecipeCommand = new RelayCommand(SignRecipe, (object sender) => RecipeWrapper != null && RecipeWrapper.Signed == false );
            PrintRecipeCommand = new RelayCommand(PrintRecipe, (object sender) => RecipeWrapper != null && RecipeWrapper.Signed == true );

            OnRecipeSigned += RecipeSigned;

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

            MarkNotificationAsRead();

        }

        private void PrintRecipe(object parameter)
        {

        }

        private void MarkNotificationAsRead()
        {
            try 
            {
                new NotificationRepository().UpdateNotificationForRecipe(_passedId);
            }
            catch(Exception e)
            {
                Logger.LogError("Error", e.ToString());
            }
        }

        private async void RecipeSigned(string signatureData)
        {
            Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(signatureData);

            if (data == null || !data.ContainsKey("recipe") || !data.ContainsKey("signature") || string.IsNullOrEmpty(data["signature"]))
            {
                Boxes.ErrorBox("Eroare in salvarea semnăturii!");
                return;
            }

            int recipeNumber = Convert.ToInt32(data["recipe"]);

            if (recipeNumber == RecipeWrapper.Id)
            {
                if (RecipeWrapper.Signed)
                {
                    Boxes.InfoBox("Rețeta este deja semnată!");
                    return;
                }

                try
                {
                    RecipeWrapper.OwnerSignature = data["signature"];
                    RecipeWrapper.Signed = true;
                    RecipeWrapper.SignedAt = DateTime.Now;
                    await new RecipeRepository().Update(RecipeWrapper.Recipe);
                    Boxes.InfoBox("Rețeta a fost semnată!");
                }
                catch (Exception e)
                {
                    Boxes.ErrorBox("Eroare in salvarea semnăturii!\n" + e.Message);
                    Logger.LogError("Error", e.ToString());
                }
            }
        }

        private void SignRecipe( object sender)
        {
            Notification notification = new Notification() 
            {
                Type = "to-sign",
                Title = RecipeWrapper.Id.ToString(),
                Message = "",
            };
            
            NotificationService.SendNotification(notification);
          
        }

        public async Task LoadRecipe()
        {
            try
            {
                Recipe recipe = await new RecipeRepository().GetById(_passedId);

                if(recipe != null)
                {

                    RecipeWrapper = new RecipeWrapper( recipe);
                    if(_navigationStore != null)
                    {
                        _navigationStore.PageTitle = "📝 Rețeta cu numărul: " + RecipeWrapper.Id;
                    }

                }

            }
            catch (Exception e)
            {
                Boxes.ErrorBox("Reteta nu a fost găsită\n" + e.Message);
            }
        }

        public async void OnLoad(object parameter)
        {
            await LoadRecipe();

        }

    }
}
