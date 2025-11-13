using PRN232_Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.ServicesMVC.Interfaces
{
    public interface IAccusationService
    {
        public Task<IEnumerable<Accusation>> GetAll();
        public Task<Accusation> Get(int id);
        public Task<bool> Add(Accusation accusation);
        public Task<bool> Update(Accusation accusation);
        public Task<bool> Delete(int id);
    }
}
