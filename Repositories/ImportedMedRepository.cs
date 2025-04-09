using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using VetManagement.Data;

namespace VetManagement.Repositories
{
    public class ImportedMedRepository : BaseRepository<ImportedMed>
    {

        public async Task Delete(string id)
        {
            var imed = await _context.ImportedMeds.FindAsync(id);
            if (imed != null)
            {
                _context.ImportedMeds.Remove(imed);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<(List<ImportedMed>, int totalPages)> GetAllFiltered(int pageNumber, int perPage, Dictionary<string, string>? filters)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }


            string nameFilter = filters != null && filters.ContainsKey("nameFilter") ? Convert.ToString(filters["nameFilter"]).ToLower() : string.Empty;
            string idFilter = filters != null && filters.ContainsKey("idFilter") ? Convert.ToString(filters["idFilter"]).ToLower() : string.Empty;
            string codeFilter = filters != null && filters.ContainsKey("codeFilter") ? Convert.ToString(filters["codeFilter"]).ToLower() : string.Empty;

            var query = _context.ImportedMeds
                .Where(m => 
                    (string.IsNullOrEmpty(nameFilter) || m.Denumire != null && m.Denumire.ToLower().StartsWith(nameFilter))
                 && (string.IsNullOrEmpty(idFilter) || m.Id != null && m.Id.ToLower().StartsWith(idFilter))
                 && (string.IsNullOrEmpty(codeFilter) || m.Cod != null && m.Cod.ToLower().StartsWith(codeFilter)))
                .OrderBy(t => t.Denumire)
                .Skip(perPage * (pageNumber - 1));

            if (perPage >= 0)
            {
                query = query.Take(perPage);
            }

            List<ImportedMed> list = await query.ToListAsync();

            int totalRecords = await query.CountAsync();

            return (list, totalRecords);
        }

        public async Task<ImportedMed> AddIfNotExists(ImportedMed model)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            var existingEntity = await _context.ImportedMeds
                .FirstOrDefaultAsync(e => e.Id == model.Id);

            if (existingEntity != null)
            {
                return null;
            }

            _context.ImportedMeds.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }


    }
}
