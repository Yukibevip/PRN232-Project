using BusinessObjects;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessObjects
{
    public class UserDAO
    {
        // Đăng nhập bằng username/password
        public  User? GetUserByUsernameAndPassword(string username, string password)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Đăng nhập/đăng ký bằng GoogleId
        public User? GetUserByGoogleId(string googleId)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.GoogleId == googleId);
        }

        // Đăng ký tài khoản mới
        public void AddUser(User user)
        {
            using var context = new CallioTestContext();
            context.Users.Add(user);
            context.SaveChanges();
        }

        // Quên mật khẩu (tìm user theo username)
        public User? GetUserByUsername(string username)
        {
            using var context = new CallioTestContext();
            return context.Users.FirstOrDefault(u => u.Username == username);
        }

        // Cập nhật mật khẩu
        public void UpdatePassword(int userId, string newPassword)
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
        // --- Admin / management DAO methods (async) ---

        public  Task<List<User>?> GetAllUsersAsync()
        {
            return _context.Users.ToListAsync();
        }

        public async Task<List<User>?> SearchUsersAsync(string? q, string? status)
        {
            IQueryable<User> query = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u =>
                    (u.Username != null && u.Username.Contains(q)) ||
                    (u.Email != null && u.Email.Contains(q)) ||
                    (u.FullName != null && u.FullName.Contains(q)));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(u => u.Status == status);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(user.Username) && await _context.Users.AnyAsync(u => u.Username == user.Username))
                return false;

            if (!string.IsNullOrWhiteSpace(user.Email) && await _context.Users.AnyAsync(u => u.Email == user.Email))
                return false;

            if (user.UserId == Guid.Empty) user.UserId = Guid.NewGuid();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var db = await _context.Users.FindAsync(user.UserId);
            if (db == null) return false;

            db.FullName = user.FullName;
            db.Email = user.Email;
            db.Gender = user.Gender;
            db.AvatarUrl = user.AvatarUrl;
            db.UserRole = user.UserRole;
            _context.Entry(db).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(Guid userId, string status)
        {
            using var context = new CallioTestContext(); // Hoặc sử dụng _context nếu đã inject
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            // THAY ĐỔI Ở ĐÂY:
            user.Status = status; // Cập nhật cột Status

            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BlockUserAsync(Guid userId)
        {
            using var context = new CallioTestContext(); // Hoặc sử dụng _context nếu đã inject
            var user = await context.Users.FindAsync(userId);
            if (user == null) return false;

            // THAY ĐỔI Ở ĐÂY:
            user.Status = "Banned"; // Cập nhật cột Status

            context.Entry(user).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]?> ExportUsersAsync(string? q, string? status)
        {
            var list = await SearchUsersAsync(q, status) ?? new List<User>();

            string Escape(string? s)
            {
                if (s == null) return "";
                if (s.Contains('"') || s.Contains(',') || s.Contains('\n'))
                {
                    return $"\"{s.Replace("\"", "\"\"")}\"";
                }
                return s;
            }

            var sb = new StringBuilder();
            sb.AppendLine("UserId,Username,FullName,Email,Gender,UserRole");
            foreach (var u in list)
            {
                sb.AppendLine($"{u.UserId},{Escape(u.Username)},{Escape(u.FullName)},{Escape(u.Email)},{Escape(u.Gender)},{Escape(u.UserRole)}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
