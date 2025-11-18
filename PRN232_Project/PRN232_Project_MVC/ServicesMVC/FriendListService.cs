using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.Models.Dto;
using PRN232_Project_MVC.ServicesMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC
{
    public class FriendListService : IFriendListService
    {
        private readonly HttpClient _httpClient;
        public FriendListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<FriendListDto>> GetFriendLists()
        {
            var response = await _httpClient.GetAsync("api/Admin/friends");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<FriendListDto>>();
            return response.IsSuccessStatusCode ? result : null;
        }

        public async Task<bool> RemoveFriendShip(Guid userId1, Guid userId2)
        {
            var response = await _httpClient.DeleteAsync($"api/Admin/friendlist?user1Id={userId1}&user2Id={userId2}");
            return response.IsSuccessStatusCode;
        }
    }
}
