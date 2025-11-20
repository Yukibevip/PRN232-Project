using System;

namespace Services.DTOs
{
    // This is used for getting conversation history
    public class MessageDto
    {
        public int MessageId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        //
        // 👇 THESE ARE THE 3 MISSING PROPERTIES YOU NEEDED TO ADD
        //
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public int? ReplyToId { get; set; } // Make it nullable (int?) to match your database
    }
}