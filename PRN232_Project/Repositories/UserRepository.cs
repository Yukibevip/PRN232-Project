using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public User? Login(string username, string password)
            => UserDAO.GetUserByUsernameAndPassword(username, password);

        public User? LoginWithGoogle(string googleId)
            => UserDAO.GetUserByGoogleId(googleId);

        public void Register(User user)
            => UserDAO.AddUser(user);

        public User? GetUserByUsername(string username)
            => UserDAO.GetUserByUsername(username);

        public void UpdatePassword(int userId, string newPassword)
            => UserDAO.UpdatePassword(userId, newPassword);
        private readonly UserDAO _userDAO;

        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO; // Injects the DAO for user data
        }

        public Task<User?> GetUserById(Guid userId)
        {
            return _userDAO.GetUserById(userId);
        }
    }
}
