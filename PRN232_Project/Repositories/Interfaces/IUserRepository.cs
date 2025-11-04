using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? Login(string username, string password);
        User? LoginWithGoogle(string googleId);
        void Register(User user);
        User? GetUserByUsername(string username);
        void UpdatePassword(int userId, string newPassword);
        Task<User?> GetUserById(Guid userId);

        // Admin / management methods
        Task<List<User>?> GetAllUsersAsync();
        Task<List<User>?> SearchUsersAsync(string? q, string? status);
        Task<bool> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ChangeStatusAsync(Guid userId, string status);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> BlockUserAsync(Guid userId);
        Task<byte[]?> ExportUsersAsync(string? q, string? status);
    }
}