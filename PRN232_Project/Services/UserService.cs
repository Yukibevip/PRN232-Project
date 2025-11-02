using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

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

        // --- Admin / management methods forward to repository ---

        public Task<List<User>?> GetAllUsersAsync()
            => _repo.GetAllUsersAsync();

        public Task<List<User>?> GetUsersAsync()
            => _repo.GetAllUsersAsync();

        public Task<List<User>?> SearchUsersAsync(string? q, string? status)
            => _repo.SearchUsersAsync(q, status);

        public Task<bool> CreateUserAsync(User user)
            => _repo.CreateUserAsync(user);

        public Task<bool> ChangeUserStatusAsync(Guid userId, string status)
            => _repo.ChangeStatusAsync(userId, status);

        public Task<bool> DeleteUserAsync(Guid userId)
            => _repo.DeleteUserAsync(userId);

        public Task<bool> BlockUserAsync(Guid userId)
            => _repo.BlockUserAsync(userId);

        public Task<byte[]?> ExportUsersAsync(string? q, string? status)
            => _repo.ExportUsersAsync(q, status);

        public Task<User?> GetUserByIdAsync(Guid id)
            => _repo.GetUserById(id);
    }
}
