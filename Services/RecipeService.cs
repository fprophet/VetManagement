using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class RecipeService
    {
        public static async Task<Recipe> FindRecipeByRegistryId(int registryId)
        {
            return await new RecipeRepository().GetRecipeByRegistryId(registryId);
        }
    }
}
