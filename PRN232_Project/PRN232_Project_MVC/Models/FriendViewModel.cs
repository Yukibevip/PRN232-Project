namespace PRN232_Project_MVC.Models
{
    public class FriendViewModel
    {
        public Guid UserId { get; set; }

        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }

    }
}
