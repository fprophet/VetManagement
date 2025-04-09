using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class ImportedMedService
    {
        public static async Task<bool> Delete<Tmed>(Tmed med) where Tmed : ImportedMed
        {
            await TreatmentHistoryService.Create<ImportedMed>(med);

            await new ImportedMedRepository().Delete(med.Id);
            return true;

        }

    }
}
