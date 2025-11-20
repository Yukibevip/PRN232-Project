using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Dto
{
    public class UserDto
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
    }
}
