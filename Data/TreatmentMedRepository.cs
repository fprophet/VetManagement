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
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }
            return await _context.TreatmentMed
                .Where(tm => tm.MedId == id)
                .Include(tm => tm.Treatment)
                .Include(tm => tm.Treatment.Patient)
                .Include(tm => tm.Treatment.Owner)
                .ToListAsync();
        }

        public async Task<List<TreatmentMed>> GetTodaysUsedMeds()
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }
            var today = DateTime.Today.AddDays(-1).Date;

            return await _context.TreatmentMed
                .Where(tm => tm.Med.LastUsed.Value.Date == today)  // Filter by today's date
                .Include(tm => tm.Med)  // Include Med details
                .GroupBy(tm => tm.Med.Name) // Group by Med Name
                .Select(g => g.First())  // Pick the first treatment for each Med Name
                .ToListAsync();

        }


    }
}