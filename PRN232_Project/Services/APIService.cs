using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class APIService
    {
        private readonly HttpClient _http;
        public APIService(HttpClient http) => _http = http;

        public async Task<User?> LoginAsync(string username, string password)
        {
            var resp = await _http.PostAsJsonAsync("api/users/login", new { Username = username, Password = password });
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<User>() : null;
        }

        public async Task<User?> LoginWithGoogleAsync(string googleId)
        {
            var resp = await _http.PostAsJsonAsync("api/users/login-google", new { GoogleId = googleId });
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<User>() : null;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var resp = await _http.GetAsync($"api/users/{userId}");
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<User?>() : null;
        }

        // added gender parameter and include in payload
        public async Task<bool> UpdateUserProfileAsync(Guid userId, string? fullname, string? email, string? avatarUrl, string? gender = null)
        {
            var payload = new
            {
                UserId = userId,
                FullName = fullname,
                Email = email,
                AvatarUrl = avatarUrl,
                Gender = gender
            };
            var resp = await _http.PostAsJsonAsync("api/users/update-profile", payload);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterAsync(string username, string email, string password, string? googleId = null)
        {
            var resp = await _http.PostAsJsonAsync("api/users/register", new { Username = username, Password = password, Email = email, GoogleId = googleId });
            return resp.IsSuccessStatusCode;
        }

        public async Task<User?> ForgotPasswordAsync(string email)
        {
            var resp = await _http.PostAsJsonAsync("api/users/forgot-password", new { Username = email });
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<User>() : null;
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            var resp = await _http.PostAsJsonAsync("api/users/reset-password", new { UserId = userId, NewPassword = newPassword });
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            var resp = await _http.PostAsJsonAsync("api/users/check-email", new { Email = email });
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordByEmailAsync(string email, string newPassword)
        {
            var resp = await _http.PostAsJsonAsync("api/users/reset-password", new { Email = email, NewPassword = newPassword });
            return resp.IsSuccessStatusCode;
        }

        // --- Admin related client methods ---

        public async Task<List<User>?> GetAllUsersAsync()
        {
            var resp = await _http.GetAsync("api/users");
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<List<User>>() : null;
        }

        public async Task<List<User>?> SearchUsersAsync(string? q, string? status = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            var url = "api/users/search" + (query.Count > 0 ? "?" + query.ToString() : "");
            var resp = await _http.GetAsync(url);
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<List<User>>() : null;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            var resp = await _http.PostAsJsonAsync("api/users", user);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeUserStatusAsync(Guid userId, string status)
        {
            var resp = await _http.PostAsJsonAsync($"api/users/change-status/{userId}", new { status });
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var resp = await _http.DeleteAsync($"api/users/{userId}");
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> AdminBlockUserAsync(Guid userId)
        {
            var resp = await _http.PostAsJsonAsync($"api/users/admin/block/{userId}", new { });
            return resp.IsSuccessStatusCode;
        }

        public string ExportUsersUrl(string? q = null, string? status = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            var url = "api/users/export" + (query.Count > 0 ? "?" + query.ToString() : "");
            return url;
        }
    }
}