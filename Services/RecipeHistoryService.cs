using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class RecipeHistoryService
    {
        public static async Task<RecipeHistory> Create(int registryId)
        {
            if (registryId <= 0)
            {
                throw new ArgumentException("Invalid registry ID", nameof(registryId));
            }

            RecipeHistory recipeHistory;

            int originalRecipeId = 0;
            try
            {
                //get the original recipe using the registry ID
                Recipe originalRecipe = await RecipeService.FindRecipeByRegistryId(registryId);

                if (originalRecipe == null)
                {
                    throw new Exception("Original recipe not found");
                }

                originalRecipeId = originalRecipe.Id;
                //create the recipe history
                recipeHistory = CreateFromOriginal(originalRecipe);

                // Save the recipe history to the database
                recipeHistory = await new BaseRepository<RecipeHistory>().Add(recipeHistory);

                //delete original recipe
                await new BaseRepository<Recipe>().Delete(originalRecipe.Id);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                throw new Exception("Eroare la arhivarea rețetei cu numărul " + originalRecipeId, ex);
            }


            return recipeHistory;
        }
        public static RecipeHistory CreateFromOriginal(Recipe originalRecipe)
        {
            if (originalRecipe == null)
            {
                throw new ArgumentNullException(nameof(originalRecipe));
            }
            return new RecipeHistory
            {
                OriginalId = originalRecipe.Id,
                OriginalRegistryNumber = originalRecipe.RegistryNumber,
                HistoryRegistryNumber = originalRecipe.RegistryNumber,
                MedName = originalRecipe.MedName,
                Date = originalRecipe.Date,
                SignedAt = originalRecipe.SignedAt,
                DeletedAt = DateTime.Now,
                Signed = originalRecipe.Signed,
                OwnerSignature = originalRecipe.OwnerSignature
            };

        }

    }

}
