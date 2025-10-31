using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Services.DTOs;

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
        }// In your APIService.cs file

        // --- Friend Methods ---
        public async Task<List<FriendDto>> GetFriendsAsync()
        {
            var response = await _http.GetAsync("api/Friends");
            if (!response.IsSuccessStatusCode)
            {
                return new List<FriendDto>(); // Return empty list on failure
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            // Deserialize into the DTO, not the ViewModel
            return JsonSerializer.Deserialize<List<FriendDto>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> UnfriendAsync(Guid friendId)
        {
            var response = await _http.DeleteAsync($"api/Friends/{friendId}");
            return response.IsSuccessStatusCode;
        }

        // --- Block Methods ---
        public async Task<bool> BlockUserAsync(Guid blockedId, int? durationInMinutes)
        {
            var blockData = new { blockedId, durationInMinutes };
            var content = new StringContent(JsonSerializer.Serialize(blockData), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/Users/block", content);
            return response.IsSuccessStatusCode;
        }
    }
}