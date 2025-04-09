using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VetManagement.Data;

namespace VetManagement.Repositories
{
    public class PatientRepository : BaseRepository<Patient>
    {

        public async Task<List<Patient>> GetForOwner(int id)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }
            return await _context.GetDbSet<Patient>().Where(p => p.OwnerId == id).ToListAsync();

        }

        public async Task<List<Patient>> GetForOwnerByType(int id, string type)
        {
            if (!await _context.CheckConnection())
            {
                throw new InvalidOperationException("Cannot connect to the database.");
            }
            return await _context.GetDbSet<Patient>().Where(p => p.OwnerId == id && p.Type == type).ToListAsync();

        }

    }
}
