using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class FriendInvitation
{
    public int InvitationId { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public DateTime SentAt { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
