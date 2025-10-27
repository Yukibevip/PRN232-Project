using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BlockList
{
    public int BlockId { get; set; }

    public Guid BlockerId { get; set; }

    public Guid BlockedId { get; set; }

    public bool IsPermanent { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public virtual User Blocked { get; set; } = null!;

    public virtual User Blocker { get; set; } = null!;
}
