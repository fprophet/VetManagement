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
                .Include(t => t.Patient) 
                .Include(t => t.TreatmentMeds) 
                .ThenInclude(tm => tm.Med) 
                .Include(t => t.Owner) 
                .Where(t => t.Patient.Type == "pet") 
                .ToListAsync();
        }        
        public async Task<List<Treatment>> GetFullTreatmentsForOwner( int id)
        {
            return await _context.Treatments
                .Include(t => t.Patient) 
                .Include(t => t.TreatmentMeds) 
                .ThenInclude(tm => tm.Med)
                .Where(t => t.OwnerId == id)
                .ToListAsync();
        }

      
    }
}
