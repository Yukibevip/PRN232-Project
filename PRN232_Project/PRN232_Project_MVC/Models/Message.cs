using System;
using System.Collections.Generic;

namespace PRN232_Project_MVC.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public DateTime ReadAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? ReplyToId { get; set; }

    public virtual ICollection<Message> InverseReplyTo { get; set; } = new List<Message>();

    public virtual User Receiver { get; set; } = null!;

    public virtual Message? ReplyTo { get; set; }

    public virtual User Sender { get; set; } = null!;
}
