using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace VetManagement.Data
{
    public class ImportedMedRepository : BaseRepository<ImportedMed>
    {


        public async Task<(List<ImportedMed>, int totalPages)> GetAllFiltered(int pageNumber, int perPage, Dictionary<string, string>? filters)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }


            string nameFilter = filters != null && filters.ContainsKey("nameFilter") ? Convert.ToString(filters["nameFilter"]).ToLower() : string.Empty;


            var query = _context.ImportedMeds
                .Where(m => (string.IsNullOrEmpty(nameFilter) || (m.Denumire != null && m.Denumire.ToLower().StartsWith(nameFilter))))
                .OrderBy(t => t.Denumire)
                .Skip(perPage * (pageNumber - 1));

            if (perPage >= 0)
            {
                query = query.Take(perPage);
            }

            List<ImportedMed> list = await  query.ToListAsync();

            int totalRecords = await _context.ImportedMeds.CountAsync();

            return (list, totalRecords);
        }

        public async Task<ImportedMed> AddIfNotExists(ImportedMed model)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            var existingEntity = await _context.ImportedMeds
                .FirstOrDefaultAsync(e =>e .Id == model.Id);

            if( existingEntity != null)
            {
                return null;
            }

            _context.ImportedMeds.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }


    }
}
