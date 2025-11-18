using PRN232_Project_MVC.Models.Dto;
using PRN232_Project_MVC.ServicesMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC
{
    public class LogService : ILogService
    {
        private readonly HttpClient _httpClient;
        public LogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<LogDto>> GetLogs()
        {
            var response = await _httpClient.GetAsync("api/admin/logs");
            var result = await response.Content.ReadFromJsonAsync<IEnumerable<LogDto>>();
            return response.IsSuccessStatusCode ? result : null;
        }
    }
}
