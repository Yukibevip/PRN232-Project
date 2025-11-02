using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _userDAO;

        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO; // Injects the DAO for user data
        }

        public User? Login(string username, string password)
            => _userDAO.GetUserByUsernameAndPassword(username, password);

        public User? LoginWithGoogle(string googleId)
            => _userDAO.GetUserByGoogleId(googleId);

        public void Register(User user)
            => _userDAO.AddUser(user);

        public User? GetUserByUsername(string username)
            => _userDAO.GetUserByUsername(username);

        public void UpdatePassword(int userId, string newPassword)
            => _userDAO.UpdatePassword(userId, newPassword);

        public Task<User?> GetUserById(Guid userId)
            => _userDAO.GetUserById(userId);

        // Admin / management delegated to DAO
        public Task<List<User>?> GetAllUsersAsync()
            => _userDAO.GetAllUsersAsync();

        public Task<List<User>?> SearchUsersAsync(string? q, string? status)
            => _userDAO.SearchUsersAsync(q, status);

        public Task<bool> CreateUserAsync(User user)
            => _userDAO.CreateUserAsync(user);

        public Task<bool> UpdateUserAsync(User user)
            => _userDAO.UpdateUserAsync(user);

        public Task<bool> ChangeStatusAsync(Guid userId, string status)
            => _userDAO.ChangeStatusAsync(userId, status);

        public Task<bool> DeleteUserAsync(Guid userId)
            => _userDAO.DeleteUserAsync(userId);

        public Task<bool> BlockUserAsync(Guid userId)
            => _userDAO.BlockUserAsync(userId);

        public Task<byte[]?> ExportUsersAsync(string? q, string? status)
            => _userDAO.ExportUsersAsync(q, status);
    }
}