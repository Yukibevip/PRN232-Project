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
    }
}
