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
    public interface IFriendListService
    {
        public Task<IEnumerable<FriendListDto>> GetFriendLists();
        Task<bool> RemoveFriendShip(Guid userId1, Guid userId2);
    }
}
