using Microsoft.AspNetCore.Mvc;
using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC.Interfaces
{
    public interface IFriendInvitationService
    {
        public Task<IEnumerable<FriendInvitationDto>> GetInvitations();
        public Task<bool> RemoveFriendInvitation(Guid user1Id, Guid user2Id);
    }
}
