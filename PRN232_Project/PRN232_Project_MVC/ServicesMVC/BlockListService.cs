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
    public class BlockListService : IBlockListService
    {
        private readonly HttpClient _httpClient;
        public BlockListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BlockListDto>> GetBlockLists()
        {
            var response = await _httpClient.GetAsync($"api/admin/blocklists");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<BlockListDto>>() : null;
        }
    }
}
