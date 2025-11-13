using PRN232_Project_MVC.Models;
using PRN232_Project_MVC.ServicesMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC
{
    public class AccusationService : IAccusationService
    {
        private readonly HttpClient _httpClient;
        public AccusationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Add(Accusation accusation)
        {
            var response = await _httpClient.PostAsJsonAsync("api/admin/accusations", accusation);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/accusations/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Accusation> Get(int id)
        {
            var response = await _httpClient.GetAsync($"api/admin/accusations/{id}");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Accusation>() : null;

        }

        public async Task<IEnumerable<Accusation>> GetAll()
        {
            var response = await _httpClient.GetAsync("api/Admin/accusations");
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<Accusation>>() : null;
        }

        public async Task<bool> Update(Accusation accusation)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/admin/accusations/{accusation.AccusationId}", accusation);
            return response.IsSuccessStatusCode;
        }
    }
}
