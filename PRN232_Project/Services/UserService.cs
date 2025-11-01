using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Services.DTOs;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IBlockListRepository _blockListRepo;

        private readonly IUserRepository _repo;
        public UserService(IUserRepository userRepo, IBlockListRepository blockListRepo)
        {
            _repo = userRepo;
            _blockListRepo = blockListRepo; // Now the tool is placed in the toolbox
        }
        public User? Login(string username, string password)
            => _repo.Login(username, password);

        public User? LoginWithGoogle(string googleId)
            => _repo.LoginWithGoogle(googleId);

        public void Register(User user)
            => _repo.Register(user);

        public User? GetUserByUsername(string username)
            => _repo.GetUserByUsername(username);

        public void UpdatePassword(int userId, string newPassword)
            => _repo.UpdatePassword(userId, newPassword);


       

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
        {
            var user = await _repo.GetUserById(userId);
            if (user == null) return null;

            // Map the User object to a UserProfileDto
            return new UserProfileDto
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl
            };
        }

        public Task BlockUserAsync(Guid blockerId, BlockUserDto blockDto)
        {
            // Business Logic: Create the BlockList object here
            var block = new BlockList
            {
                BlockerId = blockerId,
                BlockedId = blockDto.BlockedId,
                IsPermanent = !blockDto.DurationInMinutes.HasValue || blockDto.DurationInMinutes <= 0,
                ExpiresAt = blockDto.DurationInMinutes > 0
                    ? DateTime.UtcNow.AddMinutes(blockDto.DurationInMinutes.Value)
                    : null
            };
            return _blockListRepo.BlockUser(block);
        }

        public Task UnblockUserAsync(Guid blockerId, Guid blockedId)
        {
            return _blockListRepo.UnblockUser(blockerId, blockedId);
        }
        public Task<bool> IsChatBlockedAsync(Guid userId1, Guid userId2)
        {
            // Ask the repository to check for a block in either direction
            return _blockListRepo.IsBlocked(userId1, userId2);
        }
        public async Task<IEnumerable<UserProfileDto>> GetBlockedUsersAsync(Guid blockerId)
        {
            // 1. Get the list of User objects from the repository
            //    This call creates the "1 reference" on your repository method.
            var users = await _blockListRepo.GetBlockedUsers(blockerId);

            // 2. Map them to DTOs to send back to the API controller
            return users.Select(u => new UserProfileDto
            {
                UserId = u.UserId,
                Username = u.Username,
                FullName = u.FullName,
                AvatarUrl = u.AvatarUrl
            });
        }
    }
}
