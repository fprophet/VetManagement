using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class OwnerRepository : BaseRepository<Owner>
    {

        public async Task<List<Owner>> GetFullInfo()
        {
            return await _context.GetDbSet<Owner>().Include(p => p.Patients).ToListAsync();

        }


    }
}
