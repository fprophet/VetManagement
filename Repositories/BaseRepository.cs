using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context = new AppDbContext();


        public async Task<List<T>> GetAll() => await _context.GetDbSet<T>().ToListAsync();

        public async Task<T> GetById(int id)
        {
            var property = typeof(T).GetProperty("Id");
            if (property == null)
            {
                return null;
            }

            return await _context.GetDbSet<T>().FirstOrDefaultAsync(itm => EF.Property<int>(itm, "Id") == id);
        }

        public async Task<T> Add(T model)
        {
            _context.GetDbSet<T>().Add(model);
            await _context.SaveChangesAsync();

            return model;
        }


        public async Task Update(T model)
        {
            _context.GetDbSet<T>().Update(model);
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.GetDbSet<T>().FindAsync(id);
            if (user != null)
            {
                _context.GetDbSet<T>().Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T?> LastRecord()
        {
            var property = typeof(T).GetProperty("Id");

            if (property == null)
            {
                return null;
            }

            return await _context.GetDbSet<T>()
            .OrderByDescending(x => EF.Property<int>(x, "Id"))
            .FirstOrDefaultAsync();
        }


    }
}
