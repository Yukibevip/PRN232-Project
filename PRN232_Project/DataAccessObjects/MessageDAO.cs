using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class MessageDAO
    {
        private readonly CallioTestContext _context;
        public MessageDAO(CallioTestContext context) { _context = context; }

        public Task Add(Message message)
        {
            _context.Messages.Add(message);
            return _context.SaveChangesAsync();
        }

        // Add this method to get the chat history
        public Task<List<Message>> GetConversationHistory(Guid userId1, Guid userId2)
        {
            return _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                             (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt) // Order messages chronologically
                .ToListAsync();
        }
    }
}
