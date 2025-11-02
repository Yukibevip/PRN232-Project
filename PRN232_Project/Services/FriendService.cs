using Repositories.Interfaces;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendInvitationRepository _invitationRepo;
        private readonly IFriendListRepository _friendListRepo;
        private readonly IBlockListRepository _blockListRepo; // 👈 ADD THIS
        public FriendService(IFriendInvitationRepository invitationRepo, IFriendListRepository friendListRepo, IBlockListRepository blockListRepo)
        {
            _invitationRepo = invitationRepo;
            _friendListRepo = friendListRepo;
            _blockListRepo = blockListRepo;
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
            var dtos = new List<UserProfileDto>();

            foreach (var user in friends)
            {
                // Here is the new logic!
                var isBlocked = await _blockListRepo.IsUserBlockedBy(currentUserId, user.UserId);

                dtos.Add(new UserProfileDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    AvatarUrl = user.AvatarUrl,
                    IsBlocked = isBlocked // We add the new property here
                });
            }
            return dtos;
        }

        public Task UnfriendAsync(Guid currentUserId, Guid friendId)
        {
            return _friendListRepo.Unfriend(currentUserId, friendId);
        }
    }
}
