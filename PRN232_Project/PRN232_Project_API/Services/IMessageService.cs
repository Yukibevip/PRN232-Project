using BusinessObjects;
using PRN232_Project_API.DTOs;

namespace PRN232_Project_API.Services
{
    public interface IMessageService
    {
        Task CreateMessageAsync(Guid senderId, SendMessageDto messageDto);
        Task<List<Message>> GetConversationAsync(Guid userId1, Guid userId2);
    }
}