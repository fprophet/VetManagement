using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;

namespace VetManagement.Services
{
    public interface IOwnerService
    {
        Task<List<Owner>> GetOwnersAsync();
    }
}
