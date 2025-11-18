using System;
using System.Collections.Generic;

namespace BusinessObjects.Dto;

public partial class FriendInvitationDto
{
    public int InvitationId { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public DateTime SentAt { get; set; }

    public virtual UserDto Receiver { get; set; } = null!;

    public virtual UserDto Sender { get; set; } = null!;
}
