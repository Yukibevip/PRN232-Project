using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;

namespace DataAccessObjects
{
    public class UserDAO
    {
        // Đăng nhập bằng username/password
        public static User? GetUserByUsernameAndPassword(string username, string password)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Đăng nhập/đăng ký bằng GoogleId
        public static User? GetUserByGoogleId(string googleId)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.GoogleId == googleId);
        }

        // Đăng ký tài khoản mới
        public static void AddUser(User user)
        {
            using var context = new CallioTestContext();
            context.Users.Add(user);
            context.SaveChanges();
        }

        // Quên mật khẩu (tìm user theo username)
        public static User? GetUserByUsername(string username)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        // Cập nhật mật khẩu
        public static void UpdatePassword(int userId, string newPassword)
        {
            using var context = new CallioTestContext();
            var user = context.Users.Find(userId);
            if (user != null)
            {
                user.Password = newPassword;
                context.SaveChanges();
            }
        }
        private readonly CallioTestContext _context;
        public UserDAO(CallioTestContext context) { _context = context; }

        // This method is for searching your friend list
        public Task<List<User>> GetUsersByIdsAndUsername(List<Guid> userIds, string username)
        {
            return _context.Users
               .Where(u => userIds.Contains(u.UserId) && u.Username.Contains(username))
               .ToListAsync();
        }

        // Add this method for the "View friend profile" task
        public Task<User?> GetUserById(Guid userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
