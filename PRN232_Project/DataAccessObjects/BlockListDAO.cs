using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class BlockListDAO
    {
        private readonly CallioTestContext _context;
        public BlockListDAO(CallioTestContext context) { _context = context; }

        public Task<BlockList?> GetBlockRecord(Guid blockerId, Guid blockedId)
        {
            return _context.BlockLists.FirstOrDefaultAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
        }

        public Task Add(BlockList block)
        {
            _context.BlockLists.Add(block);
            return _context.SaveChangesAsync();
        }

        public Task Remove(BlockList block)
        {
            _context.BlockLists.Remove(block);
            return _context.SaveChangesAsync();
        }
        public Task<bool> CheckForBlock(Guid userId1, Guid userId2)
        {
            // Check if user1 blocked user2 OR user2 blocked user1
            return _context.BlockLists.AnyAsync(b =>
                (b.BlockerId == userId1 && b.BlockedId == userId2) ||
                (b.BlockerId == userId2 && b.BlockedId == userId1)
            );
        }
        public async Task<IEnumerable<User>> GetBlockedUsers(Guid blockerId)
        {
            // 1. Get all the IDs of users blocked by the current user
            var blockedIds = await _context.BlockLists
                .Where(b => b.BlockerId == blockerId)
                .Select(b => b.BlockedId)
                .ToListAsync();

            // 2. If there are no blocked IDs, return an empty list
            if (blockedIds == null || !blockedIds.Any())
            {
                return new List<User>();
            }

            // 3. Return the full User objects for those IDs
            return await _context.Users
                .Where(u => blockedIds.Contains(u.UserId))
                .ToListAsync();
        }
        public Task<bool> IsUserBlockedBy(Guid blockerId, Guid blockedId)
        {
            // This is a one-way check
            return _context.BlockLists.AnyAsync(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
        }
    }
}
