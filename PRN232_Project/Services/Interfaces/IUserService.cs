using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Services.DTOs;

namespace Services.Interfaces
{
    public interface IUserService
    {
        User? Login(string username, string password);
        User? LoginWithGoogle(string googleId);
        void Register(User user);
        User? GetUserByUsername(string username);
        void UpdatePassword(int userId, string newPassword);
        Task<UserProfileDto?> GetUserProfileAsync(Guid userId);
        Task BlockUserAsync(Guid blockerId, BlockUserDto blockDto);
        Task UnblockUserAsync(Guid blockerId, Guid blockedId);
        public Task<bool> IsChatBlockedAsync(Guid userId1, Guid userId2);
        Task<IEnumerable<UserProfileDto>> GetBlockedUsersAsync(Guid blockerId);

    }
}
