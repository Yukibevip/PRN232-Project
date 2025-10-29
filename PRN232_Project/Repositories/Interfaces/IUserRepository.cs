using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? Login(string username, string password);
        User? LoginWithGoogle(string googleId);
        void Register(User user);
        User? GetUserByUsername(string username);
        void UpdatePassword(int userId, string newPassword);
    }
}
