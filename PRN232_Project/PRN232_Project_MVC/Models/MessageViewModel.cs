
namespace PRN232_Project_MVC.Models
{
    // A simple class to hold message data for the view
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
