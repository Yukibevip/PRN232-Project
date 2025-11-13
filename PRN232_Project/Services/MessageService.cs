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

        public async Task<List<MessageDto>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            // 1. Get the complex data from the repository
            var messages = await _messageRepository.GetConversationHistory(userId1, userId2);

            // 2. Map (convert) it to simple DTOs
            return messages.Select(m => new MessageDto
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsRead = m.IsRead,
                IsDeleted = m.IsDeleted,
                ReplyToId = m.ReplyToId
            }).ToList();
        }
    }
    }
