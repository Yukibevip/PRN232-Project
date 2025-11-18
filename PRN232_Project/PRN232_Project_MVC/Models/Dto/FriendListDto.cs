using System;
using System.Collections.Generic;

namespace PRN232_Project_MVC.Models.Dto;

public partial class FriendListDto
{
    public Guid UserId1 { get; set; }

    public Guid UserId2 { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserDto UserId1Navigation { get; set; } = null!;

    public virtual UserDto UserId2Navigation { get; set; } = null!;
}
