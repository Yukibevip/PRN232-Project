namespace Services.DTOs
{
    // This is the generic data object the APIService will use
    public class FriendDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string? AvatarUrl { get; set; }
    }
}