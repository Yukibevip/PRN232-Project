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
    public class FriendInvitationService : IFriendInvitationService
    {
        private readonly HttpClient _httpClient;
        public FriendInvitationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<FriendInvitationDto>> GetInvitations()
        {
            var response = await _httpClient.GetAsync("api/admin/invitations");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<FriendInvitationDto>>();
            return response.IsSuccessStatusCode ? result : null;
        }

        public async Task<bool> RemoveFriendInvitation(Guid user1Id, Guid user2Id)
        {
            var response = await _httpClient.DeleteAsync($"api/Admin/friendinivtation?user1Id={user1Id}&user2Id={user2Id}");
            return response.IsSuccessStatusCode;
        }
    }
}
