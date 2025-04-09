using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NPOI.SS.Formula.Functions;
using VetManagement.Data;
using VetManagement.DataWrappers;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class DuplicateObjectService
    {
        public static async Task<RegistryRecord> DuplicateRegistryRecord(RegistryRecord registryRecord)
        {
            
            if (registryRecord == null)
            {
                return null;
            }

            //create the treatment
            Treatment treatment = await DuplicateTreatment(registryRecord.Treatment);
            if( treatment == null)
            {
                return null;
            }

            //create registryRecord object and add it to the database
            RegistryRecord newRegistryRecord = new RegistryRecord()
            {
                TreatmentId = treatment.Id,

                //RecipeNumber = registryRecord.RecipeNumber,

                Date =  DateTime.Now,

                TreatmentDuration = registryRecord.TreatmentDuration,

                Outcome = registryRecord.Outcome,

                MedName = registryRecord.MedName,

                Observations = registryRecord.Observations,

                //Treatment = treatment
               
            };

            newRegistryRecord = await new RegistryRecordRepository().Add(newRegistryRecord);

            //create recipe object and add it to the database
            Recipe recipe = new Recipe()
            {
                Date = DateTime.Now,

                MedName = registryRecord.MedName,

                Signed = false,

                OwnerSignature = "",

                RegistryNumber = newRegistryRecord.Id,

            };

            recipe = await new RecipeRepository().Add(recipe);

            //update the registryRecord object with the recipe numberX
            newRegistryRecord.RecipeNumber = recipe.Id;

            await new RegistryRecordRepository().Update(newRegistryRecord);

            //for display purposes
            newRegistryRecord.Treatment = treatment;
            newRegistryRecord.Recipe = recipe;

            return newRegistryRecord;

        }

        public static async Task<Treatment> DuplicateTreatment(Treatment treatment)
        {
            //create treatment object
            Treatment newTreatment = new Treatment
            {
                PatientId = treatment.PatientId,
                OwnerId = treatment.OwnerId,
                OwnerAddress = treatment.OwnerAddress,
                PatientAge = treatment.PatientAge,
                PatientWeight = treatment.PatientWeight,
                DateAdded = DateTime.Now,
                Details = treatment.Details
            };

            // create treatmentMeds objects and check if the quantity is available in stock
            List<TreatmentMed> treatmentMeds = DuplicateTreatmentMeds(treatment);

            // create treatmentImportedMeds objects
            List<TreatmentImportedMed> treatmentImportedMeds = DuplicateTreatmentImportedMeds(treatment);

            //create treatment and treatmentMeds objects in the database and update Meds
            newTreatment = await new TreatmentRepository().Add(newTreatment);

            treatmentMeds = await AddDuplicatedTreatmentMeds(treatmentMeds, newTreatment, treatment);

            treatmentImportedMeds = await AddDuplicatedTreatmentImportedMeds(treatmentImportedMeds, newTreatment, treatment);

            treatmentImportedMeds.ForEach(async (ntim) =>
            {
                Trace.WriteLine(ntim.ImportedMed.Denumire);
            });

            newTreatment.TreatmentMeds = treatmentMeds;
            newTreatment.TreatmentImportedMeds = treatmentImportedMeds;
            newTreatment.Owner = treatment.Owner;
            newTreatment.Patient = treatment.Patient;

            return newTreatment;
        }

        public static async Task<List<TreatmentImportedMed>> AddDuplicatedTreatmentImportedMeds(List<TreatmentImportedMed> treatmentImportedMeds, Treatment newTreatment, Treatment treatment)
        {
            foreach (TreatmentImportedMed ntim in treatmentImportedMeds)
            {
                try
                {
                    ntim.TreatmentId = newTreatment.Id;
                    await new BaseRepository<TreatmentImportedMed>().Add(ntim);

                    ImportedMed importedMed = treatment.TreatmentImportedMeds.Find(tm => tm.ImportedMedId == ntim.ImportedMedId).ImportedMed;
                    ntim.ImportedMed = importedMed;
                }
                catch (Exception ex)
                {
                    await TreatmentService.Delete(newTreatment.Id);
                    Logger.LogError("Error", ex.ToString());
                    throw new Exception(ex.Message);
                }
            }

                return treatmentImportedMeds;
        }

        public static async Task<List<TreatmentMed>> AddDuplicatedTreatmentMeds(List<TreatmentMed> treatmentMeds, Treatment newTreatment, Treatment treatment)
        {
            foreach(TreatmentMed ntm in treatmentMeds)
            {
                try
                {
                    ntm.TreatmentId = newTreatment.Id;
                    await new TreatmentMedRepository().Add(ntm);

                    //get the med object from the old TM list and update it
                    Med med = treatment.TreatmentMeds.Find(tm => tm.MedId == ntm.MedId).Med;
                    ntm.Med = med;

                    med.TotalAmount = med.TotalAmount - ntm.Quantity;
                    med.Pieces = (int)Math.Ceiling(med.TotalAmount / med.PerPiece);

                    await new MedRepository().Update(med);
                }
                catch (Exception ex)
                {
                    await TreatmentService.Delete(newTreatment.Id);
                    Logger.LogError("Error", ex.ToString());
                    throw new Exception(ex.Message);
                }
            };

            return treatmentMeds;
        }

        public static List<TreatmentImportedMed> DuplicateTreatmentImportedMeds(Treatment treatment)
        {
            List<TreatmentImportedMed> treatmentImportedMeds = new List<TreatmentImportedMed>();

            foreach (TreatmentImportedMed tm in treatment.TreatmentImportedMeds)
            {
                TreatmentImportedMed newTreatmentImportedMed = new TreatmentImportedMed
                {
                    ImportedMedId = tm.ImportedMedId,
                    //TreatmentId = 
                    Administration = "",
                    Quantity = tm.Quantity,
                    //Med = tm.Med
                };

                treatmentImportedMeds.Add(newTreatmentImportedMed);
            }

            return treatmentImportedMeds;
        }

        public static List<TreatmentMed> DuplicateTreatmentMeds(Treatment treatment)
        {
            List<TreatmentMed> treatmentMeds = new List<TreatmentMed>();
            foreach (TreatmentMed tm in treatment.TreatmentMeds)
            {
                TreatmentMed newTreatmentMed = new TreatmentMed
                {
                    MedId = tm.MedId,
                    //TreatmentId = 
                    Quantity = tm.Quantity,
                    Valability = tm.Valability,
                    //Med = tm.Med
                };

                if (newTreatmentMed.Quantity > tm.Med.TotalAmount)
                {
                    Boxes.InfoBox("Cantitatea pentru " + tm.Med.Name + " este mai mare decât cea din stoc! Tratamentul nu a fost creat!\n" +
                        "Cantitatea cerută: " + newTreatmentMed.Quantity + " " + tm.Med.MeasurmentUnit + "\n" +
                        "Cantitatea din stoc: " + tm.Med.TotalAmount + " " + tm.Med.MeasurmentUnit);
                    return null;
                }

                treatmentMeds.Add(newTreatmentMed);
            }

            return treatmentMeds;
        }
    }
}
