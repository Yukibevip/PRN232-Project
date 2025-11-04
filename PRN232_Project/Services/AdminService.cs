using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BusinessObjects;
using Services.Interfaces;

namespace Services
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _http;

        public AdminService(HttpClient http) => _http = http;

        public async Task<List<User>?> GetUsersAsync()
        {
            var resp = await _http.GetAsync("api/admin/users");
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<List<User>>() : null;
        }

        public async Task<List<User>?> SearchUsersAsync(string? q, string? status)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            var url = "api/admin/users/search" + (query.Count > 0 ? "?" + query.ToString() : "");
            var resp = await _http.GetAsync(url);
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<List<User>>() : null;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            var resp = await _http.PostAsJsonAsync("api/admin/users", user);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeUserStatusAsync(Guid userId, string status)
        {
            var resp = await _http.PostAsJsonAsync($"api/admin/users/change-status/{userId}", new { status });
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var resp = await _http.DeleteAsync($"api/admin/users/{userId}");
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> BlockUserAsync(Guid userId)
        {
            var resp = await _http.PostAsync($"api/admin/users/block/{userId}", null);
            return resp.IsSuccessStatusCode;
        }

        public async Task<byte[]?> ExportUsersAsync(string? q, string? status)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrWhiteSpace(q)) query["q"] = q;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            var url = "api/admin/users/export" + (query.Count > 0 ? "?" + query.ToString() : "");
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadAsByteArrayAsync();
        }
    }
}