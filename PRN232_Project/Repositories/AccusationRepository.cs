using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AccusationRepository : IAccusationRepository
    {
        public async Task<bool> Add(Accusation accusation)
        {
            return await AccusationDAO.Add(accusation);
        }

        public async Task<bool> Delete(int id)
        {
            return await AccusationDAO.Delete(id);
        }

        public async Task<Accusation> Get(int id)
        {
            return await AccusationDAO.Get(id);
        }

        public async Task<IEnumerable<Accusation>> GetAll()
        {
            return await AccusationDAO.GetAll();
        }

        public async Task<bool> Update(Accusation accusation)
        {
            return await AccusationDAO.Update(accusation);
        }
    }
}
