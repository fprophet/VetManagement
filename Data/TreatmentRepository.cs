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
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

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
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            string? ownerNameFilter = filters.ContainsKey("ownerName") && filters["ownerName"] != null ? Convert.ToString(filters["ownerName"]).ToLower() : string.Empty;
            string? patientSpeciesFilter = filters.ContainsKey("patientSpecies") ? Convert.ToString(filters["patientSpecies"]).ToLower() : string.Empty;
            string? medNameFilter = filters.ContainsKey("medName") ? Convert.ToString(filters["medName"]).ToLower() : string.Empty;

            DateTime? dateFilter = filters.ContainsKey("dateFilter") && filters["dateFilter"]  != null 
                    ? Convert.ToDateTime(filters["dateFilter"]).Date : null;

            string? patientType = filters.ContainsKey("patientType") ? Convert.ToString(filters["patientType"]).ToLower() : null;

            List<Treatment> list = await _context.Treatments
                .Where(t =>
                       (patientType == null || t.Patient.Type == patientType)
                    && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.ToLower().StartsWith(ownerNameFilter))
                    && (dateFilter == null || t.DateAdded.Date == dateFilter)
                    && (string.IsNullOrEmpty(patientSpeciesFilter) || t.Patient.Species.ToLower().StartsWith(patientSpeciesFilter))
                    && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.ToLower().StartsWith(medNameFilter))))

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
                    && (string.IsNullOrEmpty(patientSpeciesFilter) || (t.Patient.Name != null && t.Patient.Name.StartsWith(patientSpeciesFilter)))
                    && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.StartsWith(medNameFilter))))
                .CountAsync();


            return (list, totalRecords);
        }

        public async Task<List<Treatment>> GetFullTreatmentsForOwner( int id)
        {

            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Treatments
                .Include(t => t.Patient) 
                .Include(t => t.TreatmentMeds) 
                    .ThenInclude(tm => tm.Med)
                .Where(t => t.OwnerId == id)
                .ToListAsync();
        }

        public async Task<List<Treatment>> GetByMedName(string medName)
        {

            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Treatments
                .Include(t => t.Patient)
                .Include(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .Include(t => t.Owner)
                .Where(t => t.TreatmentMeds.Any(tm => tm.Med.Name.IndexOf(medName.Trim()) == 0 ))
                .ToListAsync();
        }

        public async Task<List<Treatment>> GetTodaysTreatments()
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Treatments
                .Where(t => t.DateAdded.Date == DateTime.Now.Date && t.Patient.Type == "pet")
                .Include(t => t.Patient)
                .Include(t => t.Owner)
                .Include(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .ToListAsync();
        }

        public new async Task<Treatment> GetById(int id)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.Treatments
                .Include(t => t.Owner)
                .Include(t => t.Patient)
                .Include(t => t.TreatmentMeds)
                   .ThenInclude(tm => tm.Med)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
        }



    }
}
