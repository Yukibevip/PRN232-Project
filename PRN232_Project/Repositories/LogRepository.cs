using BusinessObjects.Dto;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly LogDAO _logDAO;
        public LogRepository(LogDAO logDAO)
        {
            _logDAO = logDAO;
        }
        public async Task<IEnumerable<LogDto>> GetLogs()
        {
            return await _logDAO.GetLogs();
        }
    }
}
