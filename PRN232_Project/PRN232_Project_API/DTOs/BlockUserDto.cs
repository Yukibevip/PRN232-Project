namespace PRN232_Project_API.DTOs
{
    public class BlockUserDto
    {
        public Guid BlockedId { get; set; }
        // Duration in minutes. Null or 0 means permanent.
        // e.g., 5, 15, 60 (1hr), 480 (8hr), 1440 (24hr)
        public int? DurationInMinutes { get; set; }
    }
}
