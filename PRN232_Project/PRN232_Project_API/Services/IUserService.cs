using PRN232_Project_API.DTOs;

namespace PRN232_Project_API.Services
{
    public interface IUserService
    {
        Task<UserProfileDto?> GetUserProfileAsync(Guid userId);
        Task BlockUserAsync(Guid blockerId, BlockUserDto blockDto);
        Task UnblockUserAsync(Guid blockerId, Guid blockedId);
    }
   
}
