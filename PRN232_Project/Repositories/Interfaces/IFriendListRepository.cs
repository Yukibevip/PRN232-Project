using BusinessObjects;
using BusinessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFriendListRepository
    {
        Task<bool> AreUsersFriends(Guid userId1, Guid userId2);
        Task<List<User>> GetFriendsByUsername(Guid userId, string username);
        Task Unfriend(Guid userId1, Guid userId2);
        Task<IEnumerable<FriendListDto>> GetFriendLists();
        Task<bool> RemoveFriendShip(Guid userId1, Guid userId2);
    }
}
