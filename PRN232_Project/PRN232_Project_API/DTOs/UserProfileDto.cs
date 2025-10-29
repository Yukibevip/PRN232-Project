namespace PRN232_Project_API.DTOs
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
