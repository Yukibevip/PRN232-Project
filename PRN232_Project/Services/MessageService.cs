using BusinessObjects;
using Repositories.Interfaces;
using Services.DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

      

        public Task CreateMessageAsync(Guid senderId, SendMessageDto messageDto)
        {
            var message = new Message
            {
                SenderId = senderId,
                ReceiverId = messageDto.ReceiverId,
                Content = messageDto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                IsDeleted = false
            };
            return _messageRepository.SendMessage(message);
        }

        public Task<List<Message>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            return _messageRepository.GetConversationHistory(userId1, userId2);
        }
    }
}
