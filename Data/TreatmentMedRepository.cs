using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class TreatmentMedRepository : BaseRepository<TreatmentMed>
    {

        public async Task<List<TreatmentMed>> GetTreatmentsForMed(int id)
        {
            return await _context.TreatmentMed
                .Where(tm => tm.MedId == id)
                .Include(tm => tm.Treatment)
                .Include(tm => tm.Treatment.Patient)
                .Include(tm => tm.Treatment.Owner)
                .ToListAsync();
        }


    }
}