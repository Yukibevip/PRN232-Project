using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<User>?> GetUsersAsync();
        Task<List<User>?> SearchUsersAsync(string? q, string? status);
        Task<bool> CreateUserAsync(User user);
        Task<bool> ChangeUserStatusAsync(Guid userId, string status);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> BlockUserAsync(Guid userId);
        Task<byte[]?> ExportUsersAsync(string? q, string? status);
    }
}
