using BusinessObjects;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MessageRepository : IMessageRepository
    {
    private readonly MessageDAO _messageDAO;
    private readonly FriendListDAO _friendListDAO; // To check if users are friends

    public MessageRepository(MessageDAO messageDAO, FriendListDAO friendListDAO)
    {
        _messageDAO = messageDAO;
        _friendListDAO = friendListDAO;
    }

    public async Task SendMessage(Message message)
    {
        // Business Rule: You can only message friends.
        bool areFriends = await _friendListDAO.AreUsersFriends(message.SenderId, message.ReceiverId);
        if (!areFriends)
        {
            throw new InvalidOperationException("You can only send messages to friends.");
        }
        await _messageDAO.Add(message);
    }

    public Task<List<Message>> GetConversationHistory(Guid userId1, Guid userId2)
    {
        return _messageDAO.GetConversationHistory(userId1, userId2);
    }
}
    }

