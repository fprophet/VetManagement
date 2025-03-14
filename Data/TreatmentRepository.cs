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

        public async Task<List<Treatment>> GetFullTreatments_old()
        {
            return await _context.Treatments
                .Include(t => t.Patient) 
                .Include(t => t.TreatmentMeds) 
                .ThenInclude(tm => tm.Med) 
                .Include(t => t.Owner) 
                .Where(t => t.Patient.Type == "pet") 
                .ToListAsync();
        }


        public async Task<(List<Treatment>, int)> GetFullTreatments(int pageNumber, int perPage, Dictionary<string, object> filters)
        {
            var ownerNameFilter = filters.ContainsKey("ownerName") ? (string)filters["ownerName"] : string.Empty;
            var patientNameFilter = filters.ContainsKey("patientName") ? (string)filters["patientName"] : string.Empty;
            var medNameFilter = filters.ContainsKey("medName") ? (string)filters["medName"] : string.Empty;
            var dateFilter = filters.ContainsKey("dateFilter") ? filters["dateFilter"] : null;
            var patientType = filters.ContainsKey("patientType") ? filters["patientType"] : null;

            List<Treatment> list = await _context.Treatments
                .Where(t =>
                       (patientType == null || t.Patient.Type == patientType)
                    && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.StartsWith(ownerNameFilter))
                    && (dateFilter == null || t.DateAdded.Date == ((DateTime)dateFilter).Date)
                    && (string.IsNullOrEmpty(patientNameFilter) || t.Patient.Name.StartsWith(patientNameFilter))
                    && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.StartsWith(medNameFilter))))

                .OrderByDescending(t => t.Id)
                .Skip(perPage * (pageNumber - 1))
                .Take(perPage)
                .Include(t => t.Patient)
                .Include(t => t.TreatmentMeds)
                .ThenInclude(tm => tm.Med)
                .Include(t => t.Owner)
                .ToListAsync();

            int totalRecords = await _context.Treatments
                .Where(t => t.Patient.Type == "pet"
                    && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.StartsWith(ownerNameFilter))
                    && (string.IsNullOrEmpty(patientNameFilter) || t.Patient.Name.StartsWith(patientNameFilter))
                    && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.StartsWith(medNameFilter))))
                .CountAsync();


            return (list, totalRecords);
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

        public async Task<List<Treatment>> GetByMedName(string medName)
        {
            return await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.TreatmentMeds)
                .ThenInclude(tm => tm.Med)
                .Include(t => t.Owner)
                .Where(t => t.TreatmentMeds.Any(tm => tm.Med.Name.IndexOf(medName.Trim()) == 0 ))
                .ToListAsync();
        }



    }
}
