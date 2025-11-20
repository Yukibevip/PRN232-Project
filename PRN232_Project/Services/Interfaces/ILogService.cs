using BusinessObjects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<LogDto>> GetLogs();
    }
}
