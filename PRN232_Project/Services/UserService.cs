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
    }
}
