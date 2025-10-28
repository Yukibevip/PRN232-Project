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
    }
}
