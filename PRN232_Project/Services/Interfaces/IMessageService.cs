using BusinessObjects;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        Task CreateMessageAsync(Guid senderId, SendMessageDto messageDto);
        Task<List<MessageDto>> GetConversationAsync(Guid userId1, Guid userId2);
    }
}
