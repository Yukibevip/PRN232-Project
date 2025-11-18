using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.Models.Dto;
using PRN232_Project_MVC.ServicesMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MessageDto>> GetMessages()
        {
            var response = await _httpClient.GetAsync("api/Messages/messages");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<MessageDto>>() : null;
        }
    }
}
