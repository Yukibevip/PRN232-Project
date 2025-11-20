using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dto
{
    public class AccusationDto
    {
        public int AccusationId { get; set; }

        public Guid ReportedId { get; set; }

        public Guid AccusedId { get; set; }

        public string Category { get; set; } = null!;

        public string Descriptions { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime ReviewAt { get; set; }

        public Guid? ReviewedBy { get; set; }

        public string? ResolutionNote { get; set; }

        public virtual UserDto Accused { get; set; } = null!;

        public virtual UserDto Reported { get; set; } = null!;
    }
}
