using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class RegistryRecordRepository : BaseRepository<RegistryRecord>
    {
        public async Task<List<RegistryRecord>> GetRegistryRecords()
        {
            return await _context.RegistryRecords
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.Owner)
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .ToListAsync();
        }
    }
}
