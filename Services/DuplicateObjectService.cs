using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using VetManagement.Data;
using VetManagement.DataWrappers;

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
                DateAdded = DateTime.Now,
                Details = treatment.Details
            };

            // create treatmentMeds objects and check if the quantity is available in stock
            List<TreatmentMed> treatmentMeds = new List<TreatmentMed>();

            foreach (TreatmentMed tm in treatment.TreatmentMeds)
            {
                TreatmentMed newTreatmentMed = new TreatmentMed
                {
                    MedId = tm.MedId,
                    //TreatmentId = 
                    Quantity = tm.Quantity,
                    Pieces = tm.Pieces,
                    Administration = "",
                    //Med = tm.Med
                };

                if (newTreatmentMed.Quantity > tm.Med.TotalAmount)
                {
                    Boxes.InfoBox("Cantitatea pentru " + tm.Med.Name + " este mai mare decât cea din stoc! Tratamentul nu a fost creat!\n" +
                        "Cantitatea cerută: " + newTreatmentMed.Quantity + " " + tm.Med.MeasurmentUnit + "\n" +
                        "Cantitatea din stoc: " + tm.Med.TotalAmount + " " + tm.Med.MeasurmentUnit);
                    return null ;
                }

                treatmentMeds.Add(newTreatmentMed);
            }

            //create treatment and treatmentMeds objects in the database and update Meds
            newTreatment = await new TreatmentRepository().Add(newTreatment);

            treatmentMeds.ForEach(async (ntm) =>
            {
                ntm.TreatmentId = newTreatment.Id;
                ntm = await new TreatmentMedRepository().Add(ntm);

                //get the med object from the old TM list and update it
                Med med = treatment.TreatmentMeds.Find(tm => tm.MedId == ntm.MedId).Med;
                ntm.Med = med;

                med.TotalAmount = med.TotalAmount - ntm.Quantity;
                med.Pieces = (int)Math.Ceiling(med.TotalAmount / med.PerPiece);

                await new MedRepository().Update(med);

            });

            newTreatment.TreatmentMeds = treatmentMeds;
            newTreatment.Owner = treatment.Owner;
            newTreatment.Patient = treatment.Patient;

            return newTreatment;
        }
    }
}
