using System;
using System.Collections.Generic;

namespace BusinessObjects.Dto;

public partial class BlockListDto
{
    public int BlockId { get; set; }

    public Guid BlockerId { get; set; }

    public Guid BlockedId { get; set; }

    public bool IsPermanent { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public virtual UserDto Blocked { get; set; } = null!;

    public virtual UserDto Blocker { get; set; } = null!;
}
