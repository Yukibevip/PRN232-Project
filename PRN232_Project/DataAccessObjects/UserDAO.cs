using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class UserDAO
    {
        private readonly CallioTestContext _context;
        public UserDAO(CallioTestContext context) { _context = context; }

        // This method is for searching your friend list
        public Task<List<User>> GetUsersByIdsAndUsername(List<Guid> userIds, string username)
        {
            return _context.Users
               .Where(u => userIds.Contains(u.UserId) && u.Username.Contains(username))
               .ToListAsync();
        }

        // Add this method for the "View friend profile" task
        public Task<User?> GetUserById(Guid userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
