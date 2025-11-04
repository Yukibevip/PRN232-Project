using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class User
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Gender { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; } 

    public string? AvatarUrl { get; set; }
    public string? GoogleId { get; set; }

    public string? UserRole { get; set; }
    public string? Status { get; set; }

    public virtual ICollection<Accusation> AccusationAccuseds { get; set; } = new List<Accusation>();

    public virtual ICollection<Accusation> AccusationReporteds { get; set; } = new List<Accusation>();

    public virtual ICollection<Accusation> AccusationReviewedByNavigations { get; set; } = new List<Accusation>();

    public virtual ICollection<BlockList> BlockListBlockeds { get; set; } = new List<BlockList>();

    public virtual ICollection<BlockList> BlockListBlockers { get; set; } = new List<BlockList>();

    public virtual ICollection<FriendInvitation> FriendInvitationReceivers { get; set; } = new List<FriendInvitation>();

    public virtual ICollection<FriendInvitation> FriendInvitationSenders { get; set; } = new List<FriendInvitation>();

    public virtual ICollection<FriendList> FriendListUserId1Navigations { get; set; } = new List<FriendList>();

    public virtual ICollection<FriendList> FriendListUserId2Navigations { get; set; } = new List<FriendList>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();
}
