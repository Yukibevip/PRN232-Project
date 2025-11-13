using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Accusation
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

    public virtual User Accused { get; set; } = null!;

    public virtual User Reported { get; set; } = null!;

    public virtual User ReviewedByNavigation { get; set; } = null!;
}
