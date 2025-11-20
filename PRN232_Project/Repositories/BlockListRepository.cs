using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class BlockListRepository : IBlockListRepository
    {
        private readonly BlockListDAO _blockListDAO;

        public BlockListRepository(BlockListDAO blockListDAO)
        {
            _blockListDAO = blockListDAO; // Injects the DAO for block list data
        }

        public async Task BlockUser(BlockList block)
        {
            // Business Rule: Prevent blocking someone who is already blocked.
            var existingBlock = await _blockListDAO.GetBlockRecord(block.BlockerId, block.BlockedId);
            if (existingBlock != null)
            {
                throw new InvalidOperationException("This user is already blocked.");
            }
            await _blockListDAO.Add(block);
        }
        public Task<bool> IsUserBlockedBy(Guid blockerId, Guid blockedId)
        {
            // This creates the "1 reference" to your new DAO method
            return _blockListDAO.IsUserBlockedBy(blockerId, blockedId);
        }
        public Task<IEnumerable<User>> GetBlockedUsers(Guid blockerId)
        {
            // This line "creates" the reference.
            // The "0" will change to "1" after you add this.
            return _blockListDAO.GetBlockedUsers(blockerId);
        }
        public async Task UnblockUser(Guid blockerId, Guid blockedId)
        {
            // Business Rule: Ensure a block record exists before trying to remove it.
            var block = await _blockListDAO.GetBlockRecord(blockerId, blockedId);
            if (block == null)
            {
                throw new KeyNotFoundException("Block record not found.");
            }
            await _blockListDAO.Remove(block);
        }
        public Task<bool> IsBlocked(Guid userId1, Guid userId2)
        {
            // Call the DAO to check if a block exists in either direction
            return _blockListDAO.CheckForBlock(userId1, userId2);
        }
    }
}
