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

            string nameFilter = filters.ContainsKey("nameFilter") ? Convert.ToString(filters["nameFilter"]).ToLower() : string.Empty;
            string patientNameFilter = filters.ContainsKey("patientNameFilter") ? Convert.ToString(filters["patientNameFilter"]).ToLower() : string.Empty;
            string detailsFilter = filters.ContainsKey("detailsFilter") ? Convert.ToString(filters["detailsFilter"]).ToString() : string.Empty;

            var query = _context.Owners
                 .Where(o => ((string.IsNullOrEmpty(nameFilter) || o.Name.ToLower().StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(patientNameFilter) || o.Patients.Any(p => p.Name.ToLower().StartsWith(patientNameFilter))
                    && (string.IsNullOrEmpty(detailsFilter) || o.Details.ToLower().StartsWith(detailsFilter)))))
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
