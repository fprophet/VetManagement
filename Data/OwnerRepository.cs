using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class OwnerRepository : BaseRepository<Owner>
    {
        public async Task<List<Owner>> GetFullInfo()
        {
            return await _context.GetDbSet<Owner>().Include(p => p.Patients).ToListAsync();

        }

        public async Task<(List<Owner>,int)> GetFullInfoFiltered(int pageNumber, int perPage, Dictionary<string,string>? filters)
        {



            var nameFilter = filters.ContainsKey("nameFilter") ? filters["nameFilter"] : string.Empty;
            var patientNameFilter = filters.ContainsKey("patientNameFilter") ? filters["patientNameFilter"] : string.Empty;
            var detailsFilter = filters.ContainsKey("detailsFilter") ? filters["detailsFilter"] : string.Empty;

            var query = _context.Owners
                 .Where(o => ((string.IsNullOrEmpty(nameFilter) || o.Name.StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(patientNameFilter) || o.Patients.Any(p => p.Name.StartsWith(patientNameFilter))
                    && (string.IsNullOrEmpty(detailsFilter) || o.Details.StartsWith(detailsFilter)))))
                .OrderByDescending(t => t.Id)
                .Include(o => o.Patients)
                .Skip(perPage * (pageNumber - 1));

            if(perPage >= 0)
            {
                query = query.Take(perPage);
            }

            List<Owner> list = await query.ToListAsync();

            int totalRecords = await _context.Owners
                 .Where(o => ((string.IsNullOrEmpty(nameFilter) || o.Name.StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(patientNameFilter) || o.Patients.Any(p => p.Name.StartsWith(patientNameFilter))
                    && (string.IsNullOrEmpty(detailsFilter) || o.Details.StartsWith(detailsFilter)))))
                .CountAsync();

            return (list, totalRecords);
        }

    }
}
