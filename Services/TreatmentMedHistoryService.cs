using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class TreatmentMedHistoryService
    {
        public static TreatmentMedHistory CreateFromTreatmentMed(TreatmentMed treatmentMed)
        {
            TreatmentMedHistory treatmentMedHistory = new TreatmentMedHistory
            {
                OriginalMedId = Convert.ToString(treatmentMed.Med.Id),
                Name = treatmentMed.Med.Name,
                Type = treatmentMed.Med.Type,
                Quantity = Convert.ToString(treatmentMed.Quantity) + " " + treatmentMed.Med.MeasurmentUnit,
                WaitingTime = treatmentMed.Med.WaitingTime,
                Administration = treatmentMed.Med.Administration,
                InvoiceNumber = treatmentMed.Med.InvoiceNumber,
                LotID = treatmentMed.Med.LotID,
                Valability = treatmentMed.Med.Valability
            };
            return treatmentMedHistory;
        }
        
        public static TreatmentMedHistory CreateFromTreatmentImportedMed(TreatmentImportedMed treatmentMed)
        {
            TreatmentMedHistory treatmentMedHistory = new TreatmentMedHistory
            {
                OriginalMedId = treatmentMed.ImportedMedId,
                Name = treatmentMed.ImportedMed.Denumire,
                Type = "",
                Quantity = treatmentMed.Quantity,
                WaitingTime = "",
                Administration = "",
                InvoiceNumber = -1,
                LotID = "",
                Valability = null
            };
            return treatmentMedHistory;
        }

        public static async Task CreateTreatmentMedHistoryRecord(object treatmentMed, int treatmentHistoryId)
        {
            string type = treatmentMed.GetType().Name;

            TreatmentMedHistory treatmentMedHistory = new();

            if (type == "TreatmentMed")
            {
                Trace.WriteLine("Med");

                treatmentMedHistory = CreateFromTreatmentMed((TreatmentMed)treatmentMed);
            }else if(type == "TreatmentImportedMed")
            {
                Trace.WriteLine("imported");
                treatmentMedHistory = CreateFromTreatmentImportedMed((TreatmentImportedMed)treatmentMed);
                Trace.WriteLine(treatmentMedHistory.Name);

            }
            else
            {
                Trace.WriteLine("Throwing");
                throw new Exception("Unknown type: " + type);
            }

          
            try
            {
                treatmentMedHistory.TreatmentHistoryId = treatmentHistoryId;
                await new BaseRepository<TreatmentMedHistory>().Add(treatmentMedHistory);
            }
            catch (Exception ex)
            {
                Logger.LogError("HistoryLog", ex.ToString());
                throw new Exception("Error creating TreatmentMedHistory record: " + ex.Message);

            }
        }
    }
}
