using PRN232_Project_API.DTOs;

namespace PRN232_Project_API.Services
{
    public interface QIFriendService
    {
        Task SendFriendRequestAsync(Guid senderId, Guid receiverId);
        Task AcceptFriendRequestAsync(int invitationId);
        Task<IEnumerable<UserProfileDto>> GetFriendListAsync(Guid currentUserId, string? username);
        Task UnfriendAsync(Guid currentUserId, Guid friendId);
    }
}
