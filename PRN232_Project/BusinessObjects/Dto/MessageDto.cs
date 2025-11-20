namespace BusinessObjects.Dto
{
    // This is used for getting conversation history
    public class MessageDto
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
    }
}
