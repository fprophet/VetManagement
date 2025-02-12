using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetManagement.Data;

namespace VetManagement.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly AppDbContext _appDbContext;

        public OwnerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<List<Owner>> GetOwnersAsync()
        {
            return await _appDbContext.Owners.ToListAsync();
        }
    }
}
