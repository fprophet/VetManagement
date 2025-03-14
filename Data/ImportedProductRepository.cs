using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class ImportedProductRepository : BaseRepository<ImportedProduct>
    {


        public async Task<(List<ImportedProduct>, int totalPages)> GetAllFiltered(int pageNumber, int perPage, Dictionary<string, string>? filters)
        {
      
            List<ImportedProduct> list = await _context.ImportedProducts
                .OrderBy(t => t.Denumire)
                .Skip(perPage * (pageNumber - 1))
                .Take(perPage)
                .ToListAsync();

            int totalRecords = await _context.ImportedProducts.CountAsync();


            return (list, totalRecords);
        }


    }
}
