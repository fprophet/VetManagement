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
    public class PatientService
    {
        public static async Task<bool> Delete<TPatient>(TPatient patient) where TPatient : Patient
        {
            await TreatmentHistoryService.Create(patient);

            await CreatePatientHistoryRecord(patient);

            await new BaseRepository<TPatient>().Delete(patient.Id);
            return true;
        }

        public static async Task DeleteForOwner<TOwner>(TOwner owner) where TOwner : Owner
        {
            List<Patient> patients = await new PatientRepository().GetForOwner(owner.Id);

            foreach (Patient patient in patients)
            {
                 await Delete(patient);
            }
        }

        public static async Task CreatePatientHistoryRecord<TPatient>(TPatient patient) where TPatient: Patient
        {
            BaseRepository<PatientHistory> patientHistoryRepository = new();

            PatientHistory patientHistory = new PatientHistory();

            CopyPropertiesService.CopyProperties<Patient, PatientHistory>(patient, patientHistory);

            patientHistory.DateDeleted = DateTime.Now;
            patientHistory.OriginalId = patient.Id;

            try
            {
                await patientHistoryRepository.Add(patientHistory);
            }
            catch (Exception ex)
            {


                throw new Exception("Eroare la ștergerea pacientului!", ex);
            }

        }
    }
}
