using PRN232_Project_API.DTOs;
using Repositories.Interfaces;

namespace PRN232_Project_API.Services
{
    public class QFriendService : QIFriendService
    {
        private readonly IFriendInvitationRepository _invitationRepo;
        private readonly IFriendListRepository _friendListRepo;

        public QFriendService(IFriendInvitationRepository invitationRepo, IFriendListRepository friendListRepo)
        {
            _invitationRepo = invitationRepo;
            _friendListRepo = friendListRepo;
        }

        public Task SendFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            return _invitationRepo.CreateInvitation(senderId, receiverId);
        }

        public Task AcceptFriendRequestAsync(int invitationId)
        {
            return _invitationRepo.AcceptInvitation(invitationId);
        }

        public async Task<IEnumerable<UserProfileDto>> GetFriendListAsync(Guid currentUserId, string? username)
        {
            var friends = await _friendListRepo.GetFriendsByUsername(currentUserId, username ?? string.Empty);
            // Map the User business object to the public-facing DTO
            return friends.Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                Username = u.Username,
                FullName = u.FullName,
                AvatarUrl = u.AvatarUrl
            });
        }

        public Task UnfriendAsync(Guid currentUserId, Guid friendId)
        {
            return _friendListRepo.Unfriend(currentUserId, friendId);
        }
    }
}
