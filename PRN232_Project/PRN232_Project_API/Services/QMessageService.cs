using BusinessObjects;
using PRN232_Project_API.DTOs;
using Repositories.Interfaces;

namespace PRN232_Project_API.Services
{
    public class QMessageService : QIMessageService
    {
        private readonly IMessageRepository _messageRepo;

        public QMessageService(IMessageRepository messageRepo)
        {
            _messageRepo = messageRepo;
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
            return _messageRepo.SendMessage(message);
        }

        public Task<List<Message>> GetConversationAsync(Guid userId1, Guid userId2)
        {
            return _messageRepo.GetConversationHistory(userId1, userId2);
        }
    }
}