using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mysqlx.Crud;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class TreatmentHistoryService
    {

        public static async Task Create<T>(T requiredObject)
        {

            //get the treatments used by the object
            List<Treatment> treatments = await TreatmentService.GetTreatmentsByObject<T>(requiredObject);

            if (treatments == null || treatments.Count == 0)
            {
                return;
            }

            List<TreatmentHistory> treatmentHistories = new List<TreatmentHistory>();

            try
            {
                //try and create the achived records
                foreach (Treatment treatment in treatments)
                {
                    TreatmentHistory treatmentHistory =  await CreateTreatmentHistoryRecord(treatment);

                    treatmentHistories.Add(treatmentHistory);

                    if(treatmentHistory.PatientType == "livestock")
                    {
                        //create registry record history
                        await RegistryRecordHistoryService.Create(treatmentHistory);
                    }
                }
            }
            catch(Exception ex)
            {
                //on failure delete everything that has been created so far
                foreach (TreatmentHistory treatmentHistory in treatmentHistories)
                {
                    await Delete(treatmentHistory);
                }

                Logger.LogError("HistoryLog", ex.ToString());
                throw new Exception("Eroare la arhivarea tratamentelor!",ex);
            }


            //delete the original treamtents
            foreach (TreatmentHistory treatmentHistory in treatmentHistories)
            {
                await TreatmentService.Delete(treatmentHistory.OriginalId);
            }

        }

        private static async Task<TreatmentHistory> CreateTreatmentHistoryRecord(Treatment treatment)
        {
            BaseRepository<TreatmentHistory> treatmentHistoryRepository = new();
           
            TreatmentHistory treatmentHistory = CreateFromTreatment(treatment);
         
            try
            {
                treatmentHistory = await treatmentHistoryRepository.Add(treatmentHistory);
            }
            catch (Exception ex)
            {
                Logger.LogError("HistoryLog", ex.ToString());
                throw new Exception("Eroare la arhivarea tratamentelor!", ex);
            }

            try
            {
                //create treatment med history records
                foreach (TreatmentMed treatmentMed in treatment.TreatmentMeds)
                {
                    await TreatmentMedHistoryService.CreateTreatmentMedHistoryRecord(treatmentMed, treatmentHistory.Id);
                }

                //create treatment imported med history records
                foreach (TreatmentImportedMed treatmentImportedMed in treatment.TreatmentImportedMeds)
                {
                    await TreatmentMedHistoryService.CreateTreatmentMedHistoryRecord(treatmentImportedMed, treatmentHistory.Id);
                }
            }
            catch (Exception ex)
            {
                await Delete(treatmentHistory);
                Logger.LogError("HistoryLog", ex.ToString());
                throw new Exception("Eroare la arhivarea tratamentelor!",ex);
            }

            return treatmentHistory;

        }

        private static async Task Delete(TreatmentHistory treatmentHistory)
        {
            BaseRepository<TreatmentHistory> treatmentHistoryRepository = new();
            try
            {
                await treatmentHistoryRepository.Delete(treatmentHistory.Id);
            }
            catch (Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
                throw new Exception("Eroare la arhivarea tratamentelor!", ex);
            }
        }

        private static TreatmentHistory CreateFromTreatment(Treatment treatment)
        {
            TreatmentHistory treatmentHistory = new TreatmentHistory()
            {
                DateDeleted = DateTime.Now,
                DateAdded = treatment.DateAdded,
                OriginalId = treatment.Id,
                Details = treatment.Details,

                PatientId = treatment.Patient.Id,
                PatientIdentifier = Convert.ToString(treatment.Patient.Identifier) ?? treatment.Patient.Name,
                PatientType = treatment.Patient.Type,
                PatientSpecies = treatment.Patient.Species,
                PatientSex = treatment.Patient.Sex,
                PatientRace = treatment.Patient.Race,
                PatientWeight = treatment.Patient.Weight,
                PatientAge = treatment.Patient.Age,

                OwnerId = treatment.Owner.Id,
                OwnerName = treatment.Owner.Name,
                OwnerAddress = treatment.Owner.Address,
            };
            return treatmentHistory;
        }
    }
}
