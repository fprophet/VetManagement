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
                    .ThenInclude(t => t.Patient)
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .Include(rr => rr.Recipe)
                .Where(rr => rr.Treatment.Patient.Type == "livestock")
                .ToListAsync();
        }

        public async Task<(List<RegistryRecord>, int)> GetRegistryRecordsFiltered(int pageNumber, int perPage, Dictionary<string, string> filters)
        {

            string ownerNameFilter = filters.ContainsKey("ownerNameFilter") ? Convert.ToString(filters["ownerNameFilter"]).ToLower() : string.Empty;
            string patientSpeciesFilter = filters.ContainsKey("patientSpeciesFilter") ? Convert.ToString(filters["patientSpeciesFilter"]).ToLower() : string.Empty;
            string medNameFilter = filters.ContainsKey("medNameFilter") ? Convert.ToString(filters["medNameFilter"]).ToLower() : string.Empty;
            string identifierFilter = filters.ContainsKey("identifierFilter") ? Convert.ToString(filters["identifierFilter"]) : string.Empty;


            List<RegistryRecord> list = await _context.RegistryRecords

                 .Where(rr =>
                        rr.Treatment.Patient.Type == "livestock"
                    && (string.IsNullOrEmpty(ownerNameFilter) || rr.Treatment.Owner.Name.ToLower().StartsWith(ownerNameFilter))
                    && (string.IsNullOrEmpty(patientSpeciesFilter) || rr.Treatment.Patient.Species.ToLower().StartsWith(patientSpeciesFilter))
                    && (string.IsNullOrEmpty(identifierFilter) || rr.Treatment.Patient.Identifier.ToString().ToLower().StartsWith(identifierFilter))
                    && (string.IsNullOrEmpty(medNameFilter) || rr.Treatment.TreatmentMeds.Any(tm => tm.Med.Name.ToLower().StartsWith(medNameFilter))))
                .OrderByDescending(rr => rr.Id)
                .Skip(perPage * (pageNumber - 1))
                .Take(perPage)
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.Owner)
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.Patient)
                .Include(rr => rr.Treatment)
                    .ThenInclude(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .Include(rr => rr.Recipe)
                .ToListAsync();

            int totalRecords = await _context.RegistryRecords
                 .Where(rr =>
                        rr.Treatment.Patient.Type == "livestock"
                    && (string.IsNullOrEmpty(ownerNameFilter) || rr.Treatment.Owner.Name.StartsWith(ownerNameFilter))
                    && (string.IsNullOrEmpty(patientSpeciesFilter) || rr.Treatment.Patient.Species.StartsWith(patientSpeciesFilter))
                    && (string.IsNullOrEmpty(identifierFilter) || rr.Treatment.Patient.Identifier.ToString().StartsWith(identifierFilter))
                    && (string.IsNullOrEmpty(medNameFilter) || rr.Treatment.TreatmentMeds.Any(tm => tm.Med.Name.StartsWith(medNameFilter))))
                 .CountAsync();


            return (list, totalRecords);
        }
    }
}
