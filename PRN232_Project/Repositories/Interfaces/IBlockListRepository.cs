using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IBlockListRepository
    {
        Task BlockUser(BlockList block);
        Task UnblockUser(Guid blockerId, Guid blockedId);
        Task<bool> IsBlocked(Guid userId1, Guid userId2); // 👈 Add this line
        Task<IEnumerable<User>> GetBlockedUsers(Guid blockerId);
        Task<bool> IsUserBlockedBy(Guid blockerId, Guid blockedId);
    }
}
