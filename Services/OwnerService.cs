using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetManagement.Data;
using VetManagement.Repositories;

namespace VetManagement.Services
{
    public class OwnerService 
    {
        public static async Task<bool> Delete<TOwner>(TOwner owner) where TOwner : Owner
        {

            await PatientService.DeleteForOwner(owner);
            await CreateOwnerHistoryRecord(owner);
            await new BaseRepository<Owner>().Delete(owner.Id);
            // now delete the med
            return true;

        }

        public static async Task CreateOwnerHistoryRecord<TOwner>(TOwner owner) where TOwner : Owner
        {
            BaseRepository<OwnerHistory> ownerHisotryRepository = new();

            OwnerHistory ownerHisotry = new OwnerHistory();

            CopyPropertiesService.CopyProperties<Owner, OwnerHistory>(owner, ownerHisotry);

            ownerHisotry.DateDeleted = DateTime.Now;
            ownerHisotry.OriginalId = owner.Id;

            try
            {
                await ownerHisotryRepository.Add(ownerHisotry);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating MedHistory record: " + ex.Message);
            }

        }
    }
}
