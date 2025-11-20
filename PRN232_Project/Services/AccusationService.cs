using BusinessObjects;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccusationService : IAccusationService
    {
        private readonly IAccusationRepository _accusationRepository;
        public AccusationService(IAccusationRepository accusationRepository)
        {
            _accusationRepository = accusationRepository;
        }

        public async Task<bool> Add(Accusation accusation)
        {
            return await _accusationRepository.Add(accusation);
        }

        public async Task<bool> Delete(int id)
        {
            return await _accusationRepository.Delete(id);
        }

        public async Task<Accusation> Get(int id)
        {
            return await _accusationRepository.Get(id);
        }

        public async Task<IEnumerable<Accusation>> GetAll()
        {
            return await _accusationRepository.GetAll();
        }

        public async Task<bool> Update(Accusation accusation)
        {
            return await _accusationRepository.Update(accusation);
        }
    }
}
