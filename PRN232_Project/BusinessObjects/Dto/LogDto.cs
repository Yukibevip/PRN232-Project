using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dto
{
    public class LogDto
    {
        public int LogId { get; set; }

        public DateTime TimeStamp { get; set; }

        public Guid? UserId { get; set; }

        public string Action { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string? ErrorCode { get; set; }

        public virtual UserDto? User { get; set; }
    }
}
