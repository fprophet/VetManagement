using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class RegistryRecordService
    {
        public static async Task<RegistryRecord> FindByTreatmentId(int id)
        {
            // Simulate fetching from a database
            return await new RegistryRecordRepository().GetByTreatmentId(id);
        }
    }
}
