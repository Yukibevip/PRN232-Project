using BusinessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task<IEnumerable<LogDto>> GetLogs();
    }
}
