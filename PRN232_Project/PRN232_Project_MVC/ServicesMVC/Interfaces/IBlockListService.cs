using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC.Interfaces
{
    public interface IBlockListService
    {
        public Task<IEnumerable<BlockListDto>> GetBlockLists();
    }
}
