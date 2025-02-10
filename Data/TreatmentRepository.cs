using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class TreatmentRepository : BaseRepository<Treatment>
    {

        public async Task<List<Treatment>> GetFullTreatments()
        {
            return await _context.Treatments
                .Include(t => t.Patient) // Eagerly load the related Patient
                .Include(t => t.TreatmentMeds) // Eagerly load the many-to-many relationship table
                .ThenInclude(tm => tm.Med) // Load the Med associated with TreatmentMed
                .ToListAsync();
        }        
        public async Task<List<Treatment>> GetFullTreatmentsForOwner( int id)
        {
            return await _context.Treatments
                .Include(t => t.Patient) // Eagerly load the related Patient
                .Include(t => t.TreatmentMeds) // Eagerly load the many-to-many relationship table
                .ThenInclude(tm => tm.Med)
                .Where(t => t.OwnerId == id)// Load the Med associated with TreatmentMed
                .ToListAsync();
        }

      
    }
}
