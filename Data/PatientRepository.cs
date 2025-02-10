using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class PatientRepository : BaseRepository<Patient>
    {

        public async Task<List<Patient>> GetForOwner( int id)
        {
            return await _context.GetDbSet<Patient>().Where(p => p.OwnerId == id).ToListAsync();

        }


    }
}
