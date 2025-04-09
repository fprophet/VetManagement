using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class TreatmentService
    {
        public static async Task<List<Treatment>> GetTreatmentsByObject<T>(T requiredObject)
        {
            string requiredObjectType = requiredObject.GetType().Name;
            List<Treatment> treatments = new List<Treatment>();
            try
            {

                switch (requiredObjectType)
                {
                    case "Med":
                        treatments = await new TreatmentRepository().FindByModelId((Med)(object)requiredObject);
                        break;
                    case "ImportedMed":
                        treatments = await new TreatmentRepository().FindByModelId((ImportedMed)(object)requiredObject);
                        break;
                    case "Patient":
                        treatments = await new TreatmentRepository().FindByModelId((Patient)(object)requiredObject);
                        break;
                    case "Owner":
                        treatments = await new TreatmentRepository().FindByModelId((Owner)(object)requiredObject);
                        break;
                    default:
                        throw new NotImplementedException($"No history record creation implemented for type: {requiredObjectType}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("HistoryLog", ex.ToString());
                throw new Exception(ex.ToString());
            }

            return treatments;
        }

        public static async Task Delete(int id)
        {
            try
            {
                new BaseRepository<Treatment>().Delete(id);
            }
            catch(Exception ex)
            {
                Logger.LogError("Error", ex.ToString());
            }
        }

    }
}
