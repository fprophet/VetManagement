using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace VetManagement.Data
{
    public class NotificationRepository : BaseRepository<Notification>
    {
        public async Task<List<Notification>> GetForUserType(string userType)
        {
            return await _context.GetDbSet<Notification>().Where(n => n.UserType == userType && !n.Read).ToListAsync();

        }

        public async void UpdateNotificationForRecipe(int recipeNumber)
        {
            string stringNumber = Convert.ToString(recipeNumber); 
            _context.GetDbSet<Notification>()
                .Where(n => n.Title.Contains(stringNumber))
                .ExecuteUpdate( setters => setters
                                            .SetProperty(setters => setters.Read, true)
                                            .SetProperty(setters => setters.ReadAt, DateTime.Now));
        }

    }
}
