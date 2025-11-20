using AutoMapper;
using BusinessObjects;
using BusinessObjects.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class LogDAO
    {
        private readonly CallioTestContext _context;
        private readonly IMapper _mapper;
        public LogDAO(CallioTestContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LogDto>> GetLogs()
        {
            var result = await _context.Logs.Include(l => l.User).ToListAsync();
            var dto = _mapper.Map<IEnumerable<Log>, IEnumerable<LogDto>>(result);
            return dto;
        }
    }
}
