using BusinessObjects.Dto;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFriendService
    {
        Task SendFriendRequestAsync(Guid senderId, Guid receiverId);
        Task AcceptFriendRequestAsync(int invitationId);
        Task<IEnumerable<UserProfileDto>> GetFriendListAsync(Guid currentUserId, string? username);
        Task UnfriendAsync(Guid currentUserId, Guid friendId);
        Task<IEnumerable<FriendListDto>> GetFriendLists();
        Task<IEnumerable<FriendInvitationDto>> GetFriendInvitations();
        Task<bool> RemoveFriendShip(Guid userId1, Guid userId2);
        Task<bool> RemoveFriendInvitation(Guid userId1, Guid userId2);
    }
}
