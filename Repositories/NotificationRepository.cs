using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VetManagement.Services;
using VetManagement.Data;

namespace VetManagement.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public async Task<List<Notification>> GetForUserType(string userType)
        {

            if (!await _context.CheckConnection())
            {

                throw new InvalidOperationException("Cannot connect to the database.");
            }

            return await _context.GetDbSet<Notification>().Where(n => n.UserType == userType && !n.Read).ToListAsync();

        }

        public void UpdateNotificationForRecipe(int recipeNumber)
        {

            string stringNumber = Convert.ToString(recipeNumber);
            _context.GetDbSet<Notification>()
                .Where(n => n.Title.Contains(stringNumber))
                .ExecuteUpdate(setters => setters
                                            .SetProperty(setters => setters.Read, true)
                                            .SetProperty(setters => setters.ReadAt, DateTime.Now));
        }

    }
}
