using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class RegistryRecordHistoryService
    {
        public static async Task Create(TreatmentHistory treatmentHistory)
        {
            if (treatmentHistory == null)
            {
                throw new ArgumentNullException(nameof(treatmentHistory));
            }

            try
            {
                // Fetch the original registry record using the treatment ID from the treatment history
                RegistryRecord originalRegistryRecord = await RegistryRecordService.FindByTreatmentId(treatmentHistory.OriginalId);

                if (originalRegistryRecord == null)
                {
                    throw new Exception("Original registry record not found");
                }

                //create the recipe history
                RecipeHistory recipeHistory = await RecipeHistoryService.Create(originalRegistryRecord.Id);

                //create the registry record history
                RegistryRecordHistory registryRecordHistory = CreateFromOriginal(originalRegistryRecord, treatmentHistory.Id, recipeHistory.Id);

                // Save the registry record history to the database
                await new BaseRepository<RegistryRecordHistory>().Add(registryRecordHistory);

                await new BaseRepository<RegistryRecord>().Delete(originalRegistryRecord.Id);

            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log it)
                throw new Exception("Error creating registry record history", ex);

            }
        }


        public static RegistryRecordHistory CreateFromOriginal(RegistryRecord originalRegistryRecord, int historyTreatmentId, int historyRecipeId)
        {
            if (originalRegistryRecord == null)
            {
                throw new ArgumentNullException(nameof(originalRegistryRecord));
            }
            return new RegistryRecordHistory
            {
                OriginalId = originalRegistryRecord.Id,
                OriginalTreatmentId = originalRegistryRecord.TreatmentId,
                HistoryTreatmentId = historyTreatmentId,
                OriginalRecipeNumber = originalRegistryRecord.RecipeNumber,
                HistoryRecipeNumber = historyRecipeId,
                MedName = originalRegistryRecord.MedName,
                Outcome = originalRegistryRecord.Outcome,
                Observations = originalRegistryRecord.Observations,
                TreatmentDuration = originalRegistryRecord.TreatmentDuration,
            };
        }
    }
}
