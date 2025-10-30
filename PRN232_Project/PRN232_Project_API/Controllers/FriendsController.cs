using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_Project_API.DTOs;
using PRN232_Project_API.Services;

namespace PRN232_Project_API.Controllers
{
    public class FriendsController : BaseApiController
    {
        private readonly QIFriendService _friendService;

        public FriendsController(QIFriendService friendService)
        {
            _friendService = friendService;
        }

        // POST: api/friends/invite/{receiverId}
        [HttpPost("invite/{receiverId}")]
        public async Task<IActionResult> SendFriendRequest(Guid receiverId)
        {
            try
            {
                var senderId = GetCurrentUserId();
                await _friendService.SendFriendRequestAsync(senderId, receiverId);
                return Ok("Friend request sent successfully.");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message); // e.g., "Users are already friends."
            }
        }

        // POST: api/friends/accept/{invitationId}
        [HttpPost("accept/{invitationId}")]
        public async Task<IActionResult> AcceptFriendRequest(int invitationId)
        {
            try
            {
                await _friendService.AcceptFriendRequestAsync(invitationId);
                return Ok("Friend request accepted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // "Invitation not found."
            }
        }

        // GET: api/friends?username=john
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetFriendList([FromQuery] string? username)
        {
            var currentUserId = GetCurrentUserId();
            var friends = await _friendService.GetFriendListAsync(currentUserId, username);
            return Ok(friends);
        }

        // DELETE: api/friends/{friendId}
        [HttpDelete("{friendId}")]
        public async Task<IActionResult> Unfriend(Guid friendId)
        {
            var currentUserId = GetCurrentUserId();
            await _friendService.UnfriendAsync(currentUserId, friendId);
            return Ok("User has been unfriended.");
        }
    }
}