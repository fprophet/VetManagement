using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context = new AppDbContext();

        public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();

        public async Task<User> GetById(int id) => await _context.Users.FindAsync(id);

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
