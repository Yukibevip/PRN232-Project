using Microsoft.AspNetCore.Identity.UI.Services;
using PRN232_Project_MVC.Models.Dto;

namespace PRN232_Project_MVC.Models
{
    public class FriendListViewModel
    {
        public IEnumerable<FriendListDto> FriendLists { get; set; }
        public IEnumerable<FriendInvitationDto> FriendInvitations { get; set; }
        public IEnumerable<BlockListDto> BlockLists { get; set; }
        public IEnumerable<MessageDto> Messages { get; set; }
    }
}
