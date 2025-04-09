using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetManagement.Data;

namespace VetManagement.Repositories
{
    public class RecipeRepository : BaseRepository<Recipe>
    {
        public async Task<List<Recipe>> GetUnsigned()
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                .Where(r => !r.Signed).ToListAsync();
        }

        public new async Task<List<Recipe>> GetAll()
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                 .ToListAsync();
        }

        public async Task<(List<Recipe>, int)> GetAllFiltered(int pageNumber, int perPage, Dictionary<string, object>? filters)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            string ownerNameFilter = filters != null && filters.ContainsKey("ownerNameFilter") ? ((string)filters["ownerNameFilter"]).ToLower() : string.Empty;
            int? recipeNumberFilter = filters != null && filters.ContainsKey("recipeNumberFilter") && filters["recipeNumberFilter"] != null ? Convert.ToInt32(filters["recipeNumberFilter"]) : null;
            var onlyUnsignedFilter = filters != null && filters.ContainsKey("onlyUnsignedFilter") ? filters["onlyUnsignedFilter"] : true;

            List<Recipe> list = await _context.Recipes
                .Where(r =>
                    (recipeNumberFilter == null || r.Id == (int)recipeNumberFilter)
                 && ((bool)onlyUnsignedFilter == false || (bool)onlyUnsignedFilter == true && r.Signed == false)
                 && (string.IsNullOrEmpty(ownerNameFilter) ||
                    r.RegistryRecord.Treatment.Owner.Name.ToLower().StartsWith(ownerNameFilter)))

                .OrderByDescending(r => r.Id)
                .Skip(perPage * (pageNumber - 1))
                .Take(perPage)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                 .ToListAsync();

            int totalRecords = await _context.Recipes
                .Where(r =>
                    ((bool)onlyUnsignedFilter == false || (bool)onlyUnsignedFilter == true && r.Signed == false)
                && (string.IsNullOrEmpty(ownerNameFilter) || r.RegistryRecord.Treatment.Owner.Name.StartsWith(ownerNameFilter))
                && (recipeNumberFilter == null || r.Id == (int)recipeNumberFilter))
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                 .CountAsync();

            return (list, totalRecords);
        }

        public new async Task<Recipe> GetById(int id)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                  .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.TreatmentImportedMeds)
                            .ThenInclude(tm => tm.ImportedMed)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public new async Task<Recipe> GetRecipeByRegistryId(int id)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Recipes.Where(r => r.RegistryNumber == id).FirstOrDefaultAsync();
        }

        public async Task<List<Recipe>> GetTodaysSignedRecipes()
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Recipes
                .Where(r => r.Signed == true && r.SignedAt.HasValue && r.SignedAt.Value.Date == DateTime.Today.Date)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.TreatmentImportedMeds)
                            .ThenInclude(tm => tm.ImportedMed)
                .ToListAsync();
        }


    }
}
