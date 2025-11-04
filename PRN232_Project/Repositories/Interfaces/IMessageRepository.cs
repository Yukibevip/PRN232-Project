using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task SendMessage(Message message);
        Task<List<Message>> GetConversationHistory(Guid userId1, Guid userId2);
    }
}
