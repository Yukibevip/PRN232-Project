using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PRN232_Project_API
{
    // This is the real-time server
    public class ChatHub : Hub
    {
        // A thread-safe dictionary to map UserIds to SignalR ConnectionIds
        // This lets us find which "connection" belongs to which "user"
        private static readonly ConcurrentDictionary<Guid, string> UserConnections = new ConcurrentDictionary<Guid, string>();

        // This method is called by the client (JavaScript) when they connect
        public void Register(Guid userId)
                {
            // Store the user's connection ID, or update it if they reconnect
            UserConnections.AddOrUpdate(userId, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
            Console.WriteLine($"User registered in ChatHub: {userId} with connection {Context.ConnectionId}");
        }

        // This method is called by the client (JavaScript) when they send a message
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            // Convert string IDs back to GUIDs
            if (Guid.TryParse(receiverId, out Guid receiverGuid))
            {
                // Try to find the receiver's connection ID
                if (UserConnections.TryGetValue(receiverGuid, out string receiverConnectionId))
                {
                    // Send the message directly to that user's connection
                    await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderId, message, DateTime.UtcNow);
                }
                else
                {
                    // User is not online or not in the hub
                    Console.WriteLine($"Receiver {receiverId} not found or is offline.");
                }
            }
        }

        // This runs when a user disconnects
        public override async Task OnDisconnectedAsync(Exception exception)
               {
            // Find the user associated with the disconnected connection
            Guid userId = Guid.Empty;
            foreach (var entry in UserConnections)
            {
                if (entry.Value == Context.ConnectionId)
                {
                    userId = entry.Key;
                    break;
                }
            }

            // Remove them from the dictionary
            if (userId != Guid.Empty)
{
    UserConnections.TryRemove(userId, out _);
    Console.WriteLine($"User disconnected from ChatHub: {userId}");
}

await base.OnDisconnectedAsync(exception);
        }
    }
}
