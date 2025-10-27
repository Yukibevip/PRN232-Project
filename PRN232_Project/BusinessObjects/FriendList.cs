using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class FriendList
{
    public Guid UserId1 { get; set; }

    public Guid UserId2 { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User UserId1Navigation { get; set; } = null!;

    public virtual User UserId2Navigation { get; set; } = null!;
}
