using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class MedService
    {
        public static async Task<bool> Delete<Tmed>(Tmed med) where Tmed : Med
        {

            await TreatmentHistoryService.Create<Med>(med);
            await CreateMedHistoryRecord(med);

            await new BaseRepository<Med>().Delete(med.Id);
            // now delete the med
            return true;
            
        }

        public static async Task CreateMedHistoryRecord<Tmed>(Tmed med) where Tmed : Med
        {
            BaseRepository<MedHistory> medHistoryRepository = new();

            MedHistory medHistory = new MedHistory();

            CopyPropertiesService.CopyProperties<Med, MedHistory>(med, medHistory);

            medHistory.DateDeleted = DateTime.Now;
            medHistory.OriginalId = med.Id;

            try
            {
                await medHistoryRepository.Add(medHistory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating MedHistory record: " + ex.Message);
            }

        }

  
    }



}
