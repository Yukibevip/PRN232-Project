using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.DTOs;
using Services.Interfaces;

namespace PRN232_Project_API.Controllers
{
    
    public class MessagesController : BaseApiController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // POST: api/messages
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            try
            {
                var senderId = GetCurrentUserId();
                await _messageService.CreateMessageAsync(senderId, messageDto);
                return Ok("Message sent.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // "You can only send messages to friends."
            }
        }

        // GET: api/messages/{friendId}
        [HttpGet("{friendId}")]
        public async Task<IActionResult> GetConversationHistory(Guid friendId)
        {
            var currentUserId = GetCurrentUserId();
            var history = await _messageService.GetConversationAsync(currentUserId, friendId);
            return Ok(history);
        }
    }
}