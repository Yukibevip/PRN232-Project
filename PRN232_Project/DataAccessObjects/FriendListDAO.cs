using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class FriendListDAO
    {
        private readonly CallioTestContext _context;
        public FriendListDAO(CallioTestContext context)
        {
            _context = context;
        }
        public Task<bool> AreUsersFriends(Guid userId1, Guid userId2)
        {
            return _context.FriendLists.AnyAsync(f =>
                (f.UserId1 == userId1 && f.UserId2 == userId2) ||
                (f.UserId1 == userId2 && f.UserId2 == userId1));
        }
        public Task<List<Guid>> GetFriendIdsForUser(Guid userId)
        {
            return _context.FriendLists
                .Where(f => f.UserId1 == userId)
                .Select(f => f.UserId2)
                .ToListAsync();
        }
        public Task<List<FriendList>> GetFriendshipRecords(Guid userId1, Guid userId2)
        {
            return _context.FriendLists.Where(f =>
                (f.UserId1 == userId1 && f.UserId2 == userId2) ||
                (f.UserId1 == userId2 && f.UserId2 == userId1))
                .ToListAsync();
        }
        public void RemoveFriendshipRange(List<FriendList> friendshipRecords)
        {
            _context.FriendLists.RemoveRange(friendshipRecords);
        }
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        public Task AddFriendship(FriendList friendship)
        {
            _context.FriendLists.Add(friendship);
            return _context.SaveChangesAsync();
        }
    }
}
