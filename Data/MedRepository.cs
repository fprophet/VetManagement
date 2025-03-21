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
            string nameFilter = filters.ContainsKey("nameFilter") ? Convert.ToString(filters["nameFilter"]).ToLower() : string.Empty;
            string typeFilter = filters.ContainsKey("typeFilter") ? Convert.ToString(filters["typeFilter"]).ToLower() : string.Empty;
            
            DateTime? dateAddedFilter = filters.ContainsKey("dateAddedFilter") && filters["dateAddedFilter"]  != null 
                    ? Convert.ToDateTime(filters["dateAddedFilter"]).Date : null;
            DateTime? valabilityFilter = filters.ContainsKey("valabilityFilter") && filters["valabilityFilter"] != null 
                    ? Convert.ToDateTime(filters["valabilityFilter"]).Date : null;

            string lotFilter = filters.ContainsKey("lotFilter") ? Convert.ToString(filters["lotFilter"]).ToLower() : string.Empty;

            var query = _context.Meds
                .Where(m => (string.IsNullOrEmpty(nameFilter) || m.Name.ToLower().StartsWith(nameFilter))
                    && (string.IsNullOrEmpty(typeFilter) || m.Type == typeFilter)
                    && (string.IsNullOrEmpty(lotFilter) || m.LotID.StartsWith(lotFilter))
                    && (dateAddedFilter == null || m.DateAdded.Date == dateAddedFilter)
                    && (valabilityFilter == null || m.Valability.Date == valabilityFilter))
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

        public async Task<List<Med>> GetTodaysNewMeds()
        {
            var today = DateTime.Today.Date;
            return await _context.Meds
                .Where(m => m.DateAdded.Date == today.AddDays(-1).Date)
                .ToListAsync();
        }
        
   

    }
}
