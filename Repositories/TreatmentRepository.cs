using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using VetManagement.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VetManagement.Repositories
{
    public class TreatmentRepository : BaseRepository<Treatment>
    {

        public async Task<List<Treatment>> FindByModelId<T>(T requiredModel)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
            {
                throw new InvalidOperationException("The model does not have an Id property.");
            }

            //ImportedMed has string id. casting it to int would alter the value
            string id = Convert.ToString(property.GetValue(requiredModel));

            string requiredObjectType = requiredModel.GetType().Name;


            IQueryable<Treatment> query = _context.Treatments;

            switch (requiredObjectType)
            {
                case "Med":
                    query = query.Where(t => t.TreatmentMeds.Any(tm => tm.MedId == Convert.ToInt32(id)));
                    break;
                case "ImportedMed":
                    query = query.Where(t => t.TreatmentImportedMeds.Any(tm => tm.ImportedMedId == id));
                    break;
                case "Owner":
                    query = query.Where(t => t.OwnerId == Convert.ToInt32(id));
                    break;
                case "Patient":
                    query = query.Where(t => t.PatientId == Convert.ToInt32(id));
                    break;

            }

            query = query
                .Include(t => t.Patient)
                .Include(t => t.Owner)
                .Include(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
            .Include(t => t.TreatmentImportedMeds)
                    .ThenInclude(tm => tm.ImportedMed);

            return await query.ToListAsync();
        }

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

            DateTime? fromDateFilter = filters.ContainsKey("fromDateFilter") && filters["fromDateFilter"] != null
                    ? Convert.ToDateTime(filters["fromDateFilter"]).Date : null;

            DateTime? toDateFilter = filters.ContainsKey("toDateFilter") && filters["toDateFilter"] != null
                    ? Convert.ToDateTime(filters["toDateFilter"]).Date : null;

            string? patientType = filters.ContainsKey("patientType") ? Convert.ToString(filters["patientType"]).ToLower() : null;

            var query = _context.Treatments
            .Where(t =>
                   (string.IsNullOrEmpty(patientType) || t.Patient.Type == patientType)
                && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.ToLower().StartsWith(ownerNameFilter))
                && (fromDateFilter == null || t.DateAdded.Date >= fromDateFilter)
                && (toDateFilter == null || t.DateAdded.Date <= toDateFilter)
                && (string.IsNullOrEmpty(patientSpeciesFilter) || t.Patient.Species.ToLower().StartsWith(patientSpeciesFilter))
                && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.ToLower().StartsWith(medNameFilter))))
            .OrderByDescending(t => t.Id)
            .Skip(perPage * (pageNumber - 1));

            if (perPage >= 0)
            {
                query = query.Take(perPage);
            }

            List<Treatment> list = await query
                .Include(t => t.Patient)
                .Include(t => t.Owner)
                .Include(t => t.TreatmentMeds)
                    .ThenInclude(tm => tm.Med)
                .Include(t => t.TreatmentImportedMeds)
                    .ThenInclude(tm => tm.ImportedMed)
                .ToListAsync();

            int totalRecords = await _context.Treatments
                .Where(t =>
                       (string.IsNullOrEmpty(patientType) || t.Patient.Type == patientType)
                && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.ToLower().StartsWith(ownerNameFilter))
                && (fromDateFilter == null || t.DateAdded.Date >= fromDateFilter)
                && (toDateFilter == null || t.DateAdded.Date <= toDateFilter)
                && (string.IsNullOrEmpty(patientSpeciesFilter) || t.Patient.Species.ToLower().StartsWith(patientSpeciesFilter))
                && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.ToLower().StartsWith(medNameFilter))))
                .CountAsync();


            return (list, totalRecords);
        }

        public async Task<List<Treatment>> GetFullTreatmentsForOwner(int id)
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
                .Where(t => t.TreatmentMeds.Any(tm => tm.Med.Name.IndexOf(medName.Trim()) == 0))
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


        public async Task<(List<TreatmentDisplay>, int)> GetCurrentAndHistoryTreatments(int pageNumber, int perPage, Dictionary<string, object> filters)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }

             (List<TreatmentDisplay> treatmentList, int totalRecords) = await GetCurrentAndHistoryTreatmentsList(pageNumber, perPage, filters);

            if(treatmentList.Count == 0 )
            {
                return (treatmentList, 0);
            }

            var currentIds = treatmentList.Where(t => !t.IsArchived).Select(t => t.Id).ToList();
            var archivedIds = treatmentList.Where(t => t.IsArchived).Select(t => t.Id).ToList();

            List<TreatmentMedDisplay> treatmentMedLists = await GetCurrentAndHistoryTreatmentsMedLists(currentIds, archivedIds);
       
            foreach (var treatment in treatmentList)
            {
                treatment.Meds = treatmentMedLists.Where(tm => tm.IsArcihved == treatment.IsArchived && tm.TreatmentId == treatment.Id).ToList();
            }

            //now get the treatment meds
            return (treatmentList, totalRecords);
        }

        public async Task<List<TreatmentMedDisplay>> GetCurrentAndHistoryTreatmentsMedLists
            (List<int> currentIds, List<int> archivedIds)
        {
            var currentMedQuery = _context.TreatmentMed
                .Where(tm => currentIds.Contains(tm.TreatmentId))
                .Select(tm => new TreatmentMedDisplay
                {
                    TreatmentId = tm.TreatmentId,
                    IsArcihved = false,
                    Name = tm.Med.Name,
                    Quantity = Convert.ToString(tm.Quantity) + " " + tm.Med.MeasurmentUnit,
                    Valability = tm.Valability,
                    LotID = tm.Med.LotID,
                    WaitingTime = tm.Med.WaitingTime,
                    Administration = tm.Med.Administration
                });

            var currentImportedQuery = _context.TreatmentImportedMed
                .Where(tm => currentIds.Contains(tm.TreatmentId))
               .Select(tm => new TreatmentMedDisplay
               {
                   TreatmentId = tm.TreatmentId,
                   IsArcihved = false,
                   Name = tm.ImportedMed.Denumire,
                   Quantity = tm.Quantity,
                   Valability = null,
                   LotID = null,
                   WaitingTime = null,
                   Administration = null
               });

            var archivedMedQuery = _context.TreatmentMedHistory
                .Where(tm => archivedIds.Contains(tm.TreatmentHistoryId))
                .Select(tm => new TreatmentMedDisplay
                {
                    TreatmentId = tm.TreatmentHistoryId,
                    IsArcihved = true,
                    Name = tm.Name,
                    Quantity = tm.Quantity,
                    Valability = tm.Valability,
                    LotID = tm.LotID,
                    WaitingTime = tm.WaitingTime,
                    Administration = tm.Administration
                });

            var combined = currentMedQuery.Concat(currentImportedQuery).Concat(archivedMedQuery);

            return await combined.ToListAsync();
        }

        public async Task<(List<TreatmentDisplay>,int)> GetCurrentAndHistoryTreatmentsList
            (int pageNumber, int perPage, Dictionary<string, object> filters)
        {

            string? ownerNameFilter = filters.ContainsKey("ownerName") && filters["ownerName"] != null ? Convert.ToString(filters["ownerName"]).ToLower() : string.Empty;
            string? patientSpeciesFilter = filters.ContainsKey("patientSpecies") ? Convert.ToString(filters["patientSpecies"]).ToLower() : string.Empty;
            string? medNameFilter = filters.ContainsKey("medName") ? Convert.ToString(filters["medName"]).ToLower() : string.Empty;

            DateTime? fromDateFilter = filters.ContainsKey("fromDateFilter") && filters["fromDateFilter"] != null
                    ? Convert.ToDateTime(filters["fromDateFilter"]).Date : null;

            DateTime? toDateFilter = filters.ContainsKey("toDateFilter") && filters["toDateFilter"] != null
                    ? Convert.ToDateTime(filters["toDateFilter"]).Date : null;

            string? patientType = filters.ContainsKey("patientType") ? Convert.ToString(filters["patientType"]).ToLower() : null;

            var activeQuery = _context.Treatments
                 .Where(t =>
                       (string.IsNullOrEmpty(patientType) || t.Patient.Type == patientType)
                && (string.IsNullOrEmpty(ownerNameFilter) || t.Owner.Name.ToLower().StartsWith(ownerNameFilter))
                && (fromDateFilter == null || t.DateAdded.Date >= fromDateFilter)
                && (toDateFilter == null || t.DateAdded.Date <= toDateFilter)
                && (string.IsNullOrEmpty(patientSpeciesFilter) || t.Patient.Species.ToLower().StartsWith(patientSpeciesFilter))
                && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMeds.Any(tm => tm.Med.Name.ToLower().StartsWith(medNameFilter))))
         
                .Select(t => new TreatmentDisplay
                {
                    Id = t.Id,
                    IsArchived = false,
                    PatientIdentifier = t.Patient.Identifier ?? t.Patient.Name,
                    OwnerName = t.Owner.Name,
                    OwnerAddress = t.OwnerAddress,
                    PatientSpecies = t.Patient.Species,
                    PatientRace = t.Patient.Race,
                    PatientAge = t.PatientAge,
                    PatientWeight = t.PatientWeight,
                    PatientSex = t.Patient.Sex,
                    Details = t.Details,
                    DateAdded = t.DateAdded,
                });

            var historyQuery = _context.TreatmentHistory
                 .Where(t =>
                       (string.IsNullOrEmpty(patientType) || t.PatientType == patientType)
                && (string.IsNullOrEmpty(ownerNameFilter) || t.OwnerName.ToLower().StartsWith(ownerNameFilter))
                && (fromDateFilter == null || t.DateAdded.Date >= fromDateFilter)
                && (toDateFilter == null || t.DateAdded.Date <= toDateFilter)
                && (string.IsNullOrEmpty(patientSpeciesFilter) || t.PatientSpecies.ToLower().StartsWith(patientSpeciesFilter))
                && (string.IsNullOrEmpty(medNameFilter) || t.TreatmentMedHistory.Any(tm => tm.Name.ToLower().StartsWith(medNameFilter))))
                .Select(th => new TreatmentDisplay
                {
                    Id = th.Id,
                    IsArchived = true,
                    PatientIdentifier = th.PatientIdentifier,
                    OwnerName = th.OwnerName,
                    OwnerAddress = th.OwnerAddress,
                    PatientSpecies = th.PatientSpecies,
                    PatientRace = th.PatientRace,
                    PatientAge = th.PatientAge,
                    PatientWeight = th.PatientWeight,
                    PatientSex = th.PatientSex,
                    Details = th.Details,
                    DateAdded = th.DateAdded,

                });

            var combinedQuery = activeQuery
                .Union(historyQuery)
                .OrderByDescending(t => t.Id)
                .Skip(perPage * (pageNumber - 1))
                .Take(perPage);

            //var count = await activeQuery.Union(historyQuery).CountAsync();
            var list = await combinedQuery.ToListAsync();

            int totalRecords = await combinedQuery.CountAsync();
            return (list, totalRecords);
        }
    }

}