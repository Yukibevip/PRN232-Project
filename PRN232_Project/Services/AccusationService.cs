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
    }
}
