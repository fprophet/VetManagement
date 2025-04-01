using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class UserRepository : BaseRepository<User>
    {

        public async Task<User> GetByUsername(string username) => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

    }
}
