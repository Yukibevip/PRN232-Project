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

        
        public async Task<List<FriendDto>> GetFriendsAsync(string? searchInput)
        {
            // Build the query URL, adding the searchInput if it exists
            string apiUrl = "api/Friends";
            if (!string.IsNullOrEmpty(searchInput))
            {
                apiUrl += $"?username={searchInput}";
            }

            var response = await _http.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new List<FriendDto>(); // Return empty list on failure
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            // Deserialize into the DTO
            return JsonSerializer.Deserialize<List<FriendDto>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //
        // METHOD 2: FOR GETTING CHAT HISTORY (Fixes GetConversationHistoryAsync error)
        //
        public async Task<List<MessageDto>> GetConversationHistoryAsync(Guid friendId)
        {
            var response = await _http.GetAsync($"api/Messages/{friendId}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<MessageDto>();
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<MessageDto>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //
        // METHOD 3: FOR SAVING A NEW MESSAGE (Fixes SaveMessageAsync error)
        //
        public async Task<bool> SaveMessageAsync(Guid receiverId, string content)
        {
            var messageData = new { receiverId, content };
            var jsonContent = new StringContent(JsonSerializer.Serialize(messageData), Encoding.UTF8, "application/json");

            // This calls your API endpoint: POST /api/Messages
            var response = await _http.PostAsync("api/Messages", jsonContent);
            return response.IsSuccessStatusCode;
        }
        // ... inside your APIService class ...

        public async Task<bool> CheckBlockStatusAsync(Guid friendId)
        {
            // This calls a new API endpoint we are about to create
            var response = await _http.GetAsync($"api/Users/check-block/{friendId}");

            // We'll deserialize the true/false response
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                // The API will return a simple JSON like: { "isBlocked": true }
                // We parse this JSON to get the boolean value.
                try
                {
                    using (var doc = JsonDocument.Parse(jsonString))
                    {
                        return doc.RootElement.GetProperty("isBlocked").GetBoolean();
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false; // Assume not blocked if API call fails
        }

        // You also need these methods if you don't have them
        // This fixes your "Unfriend" 404 error
        public async Task<bool> UnfriendAsync(Guid friendId)
        {
            var response = await _http.DeleteAsync($"api/Friends/{friendId}");
            return response.IsSuccessStatusCode;
        }

        // This fixes your "Block" 404 error
        public async Task<bool> BlockUserAsync(Guid blockedId, int? durationInMinutes)
        {
            var blockData = new { blockedId, durationInMinutes };
            var content = new StringContent(JsonSerializer.Serialize(blockData), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/Users/block", content);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<FriendDto>> GetBlockedUsersAsync()
        {
            // 1. Call the API endpoint
            var response = await _http.GetAsync("api/Users/blocked-list");

            // 2. Check if the call was successful
            if (!response.IsSuccessStatusCode)
            {
                return new List<FriendDto>(); // Return an empty list on failure
            }

            // 3. Read the JSON response
            var jsonString = await response.Content.ReadAsStringAsync();

            // 4. Convert the JSON into a list of DTOs and return it
            //    We can re-use the FriendDto for this, as it has the same properties
            return JsonSerializer.Deserialize<List<FriendDto>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<bool> UnblockUserAsync(Guid blockedId)
        {
            // This calls your API endpoint: DELETE /api/Users/unblock/{blockedId}
            var response = await _http.DeleteAsync($"api/Users/unblock/{blockedId}");
            return response.IsSuccessStatusCode;
        }
    }
}