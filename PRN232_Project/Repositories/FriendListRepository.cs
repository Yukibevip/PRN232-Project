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
    public class FriendListRepository : IFriendListRepository
    {
        private readonly FriendListDAO _friendListDAO;
        private readonly UserDAO _userDAO;

        // The repository takes DAOs in its constructor
        public FriendListRepository(FriendListDAO friendListDAO, UserDAO userDAO)
        {
            _friendListDAO = friendListDAO;
            _userDAO = userDAO;
        }

        public Task<bool> AreUsersFriends(Guid userId1, Guid userId2)
        {
            return _friendListDAO.AreUsersFriends(userId1, userId2);
        }

        public async Task<List<User>> GetFriendsByUsername(Guid userId, string username)
        {
            // Step 1: Use a DAO to get friend IDs
            var friendIds = await _friendListDAO.GetFriendIdsForUser(userId);
            // Step 2: Use another DAO to get the User objects for those IDs
            return await _userDAO.GetUsersByIdsAndUsername(friendIds, username);
        }

        public async Task Unfriend(Guid userId1, Guid userId2)
        {
            var records = await _friendListDAO.GetFriendshipRecords(userId1, userId2);
            if (records.Any())
            {
                _friendListDAO.RemoveFriendshipRange(records);
                await _friendListDAO.SaveChangesAsync();
            }
        }
    }
}
