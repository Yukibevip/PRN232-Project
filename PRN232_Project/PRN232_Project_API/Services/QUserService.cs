using BusinessObjects;
using PRN232_Project_API.DTOs;
using Repositories.Interfaces;

namespace PRN232_Project_API.Services
{
    public class QUserService : QIUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IBlockListRepository _blockListRepo;

        public QUserService(IUserRepository userRepo, IBlockListRepository blockListRepo)
        {
            _userRepo = userRepo;
            _blockListRepo = blockListRepo;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepo.GetUserById(userId);
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
    }
}
