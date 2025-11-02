using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interfaces
{
    public interface IUserService
    {
        User? Login(string username, string password);
        User? LoginWithGoogle(string googleId);
        void Register(User user);
        User? GetUserByUsername(string username);
        void UpdatePassword(int userId, string newPassword);

        // Admin / management methods
        Task<List<User>?> GetAllUsersAsync();
        Task<List<User>?> GetUsersAsync(); // kept for compatibility with controllers that call GetUsersAsync
        Task<List<User>?> SearchUsersAsync(string? q, string? status);
        Task<bool> CreateUserAsync(User user);
        Task<bool> ChangeUserStatusAsync(Guid userId, string status);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> BlockUserAsync(Guid userId);
        Task<byte[]?> ExportUsersAsync(string? q, string? status);
        Task<User?> GetUserByIdAsync(Guid id);
    }
}
