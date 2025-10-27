using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Log
{
    public int LogId { get; set; }

    public DateTime TimeStamp { get; set; }

    public Guid? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? ErrorCode { get; set; }

    public virtual User? User { get; set; }
}
