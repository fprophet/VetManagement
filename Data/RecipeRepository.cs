﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class RecipeRepository : BaseRepository<Recipe>
    {
        public async Task<List<Recipe>> GetUnsigned()
        {
            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                .Where(r => !r.Signed).ToListAsync();
        }    
        
        public async Task<List<Recipe>> GetAll()
        {
            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                 .ToListAsync();
        }

        public async Task<(List<Recipe>,int)> GetAllFiltered( int pageNumber, int perPage, Dictionary<string,object>? filters)
        {
            var ownerNameFilter = filters.ContainsKey("ownerNameFilter") ? filters["ownerNameFilter"] : string.Empty;
            var recipeNumberFilter = filters.ContainsKey("recipeNumberFilter") ? filters["recipeNumberFilter"] : null;
            var onlyUnsignedFilter = filters.ContainsKey("onlyUnsignedFilter") ? filters["onlyUnsignedFilter"] : true;
   
            List<Recipe> list = await _context.Recipes
                .Where( r => 
                    (recipeNumberFilter != null || r.Id == (int)recipeNumberFilter)
                 && ((bool)onlyUnsignedFilter == false  || ((bool)onlyUnsignedFilter == true && r.Signed == false) )
                 && (string.IsNullOrEmpty((string)ownerNameFilter) || r.RegistryRecord.Treatment.Owner.Name.StartsWith((string)ownerNameFilter)))

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
                    ((bool)onlyUnsignedFilter == false || ((bool)onlyUnsignedFilter == true && r.Signed == false))
                && (string.IsNullOrEmpty((string)ownerNameFilter) || r.RegistryRecord.Treatment.Owner.Name.StartsWith((string)ownerNameFilter))
                && (recipeNumberFilter != null || r.Id == (int)recipeNumberFilter))
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                 .CountAsync();

            return (list, totalRecords);
        }

        public async Task<Recipe> GetById( int id)
        {
            return await _context.Recipes
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Owner)
                .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.Patient)
                  .Include(r => r.RegistryRecord)
                    .ThenInclude(rr => rr.Treatment)
                        .ThenInclude(t => t.TreatmentMeds)
                            .ThenInclude(tm => tm.Med)
                .Where(r => r.Id == id).FirstOrDefaultAsync();
        }
    }
}
