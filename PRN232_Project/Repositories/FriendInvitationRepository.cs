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
    public class FriendInvitationRepository : IFriendInvitationRepository
    {
        private readonly FriendInvitationDAO _invitationDAO;
        private readonly FriendListDAO _friendListDAO; // To add friends
        private readonly UserDAO _userDAO;             // To check if users are already friends

        public FriendInvitationRepository(FriendInvitationDAO invitationDAO, FriendListDAO friendListDAO, UserDAO userDAO)
        {
            _invitationDAO = invitationDAO;
            _friendListDAO = friendListDAO;
            _userDAO = userDAO;
        }

        public async Task CreateInvitation(Guid senderId, Guid receiverId)
        {
            // Business logic checks
            if (senderId == receiverId) throw new ArgumentException("Cannot send invitation to yourself.");
            if (await _friendListDAO.AreUsersFriends(senderId, receiverId)) throw new InvalidOperationException("Users are already friends.");
            if (await _invitationDAO.Exists(senderId, receiverId)) throw new InvalidOperationException("Invitation already exists.");

            var invitation = new FriendInvitation { SenderId = senderId, ReceiverId = receiverId };
            await _invitationDAO.Add(invitation);
        }

        public async Task AcceptInvitation(int invitationId)
        {
            var invitation = await _invitationDAO.GetById(invitationId);
            if (invitation == null) throw new KeyNotFoundException("Invitation not found.");

            // Create friendship records (both ways for easy querying)
            var friendship1 = new FriendList { UserId1 = invitation.SenderId, UserId2 = invitation.ReceiverId, CreatedAt = DateTime.UtcNow };
            var friendship2 = new FriendList { UserId1 = invitation.ReceiverId, UserId2 = invitation.SenderId, CreatedAt = DateTime.UtcNow };

            // This should be a transaction in a real app, but for now, we'll do it sequentially.
            await _friendListDAO.AddFriendship(friendship1);
            await _friendListDAO.AddFriendship(friendship2);
            await _invitationDAO.Remove(invitation);
        }
    }
}
