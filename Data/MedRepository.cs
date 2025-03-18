using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;
using K4os.Compression.LZ4.Internal;
using System.Diagnostics;

namespace VetManagement.Data
{
    public class MedRepository : BaseRepository<Med>
    {
        public async Task<(List<Med>, int)> GetAllFiltered(int pageNumber, int perPage, Dictionary<string, object> filters)
        {
            var nameFilter = filters.ContainsKey("nameFilter") ? filters["nameFilter"].ToString() : string.Empty;
            var typeFilter = filters.ContainsKey("typeFilter") ? filters["typeFilter"].ToString() : string.Empty;
            var dateAddedFilter = filters.ContainsKey("dateAddedFilter") ? filters["dateAddedFilter"] : null;
            var valabilityFilter = filters.ContainsKey("valabilityFilter") ? filters["valabilityFilter"] : null;
            var lotFilter = filters.ContainsKey("lotFilter") ? filters["lotFilter"].ToString() : string.Empty;

            var query = _context.Meds
                .Where(m => (string.IsNullOrEmpty(nameFilter) || m.Name.StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(typeFilter) || m.Type == typeFilter)
                    && (string.IsNullOrEmpty(lotFilter) || m.LotID.StartsWith(lotFilter))
                    && (dateAddedFilter == null || m.DateAdded.Date == ((DateTime)dateAddedFilter).Date)
                    && (valabilityFilter == null || m.Valability.Date == ((DateTime)valabilityFilter).Date))
                .OrderByDescending(m => m.Id)
                .Skip(perPage * (pageNumber - 1));
            
            if(perPage >= 0 )
            {
                query = query.Take(perPage);
            }

            List<Med> list = await query.ToListAsync();

            int totalRecords = await _context.Meds
                .Where(m => (string.IsNullOrEmpty(nameFilter) || m.Name.StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(typeFilter) || m.Type.StartsWith(typeFilter))
                    && (dateAddedFilter == null || m.DateAdded == (DateTime)dateAddedFilter)
                    && (valabilityFilter == null || m.Valability ==  (DateTime)valabilityFilter)
                    && (string.IsNullOrEmpty(lotFilter) || m.LotID.StartsWith(lotFilter)))
                .CountAsync();

            return (list, totalRecords);
        }

    }
}
