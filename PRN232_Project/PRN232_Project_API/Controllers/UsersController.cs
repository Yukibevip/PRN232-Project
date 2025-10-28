using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_Project_API.DTOs;
using PRN232_Project_API.Services;

namespace PRN232_Project_API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users/{userId}/profile
        [HttpGet("{userId}/profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile(Guid userId)
        {
            var userProfile = await _userService.GetUserProfileAsync(userId);
            if (userProfile == null)
            {
                return NotFound("User not found.");
            }
            return Ok(userProfile);
        }

        // POST: api/users/block
        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserDto blockDto)
        {
            try
            {
                var blockerId = GetCurrentUserId();
                await _userService.BlockUserAsync(blockerId, blockDto);
                return Ok("User blocked successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // "This user is already blocked."
            }
        }

        // DELETE: api/users/unblock/{blockedId}
        [HttpDelete("unblock/{blockedId}")]
        public async Task<IActionResult> UnblockUser(Guid blockedId)
        {
            try
            {
                var blockerId = GetCurrentUserId();
                await _userService.UnblockUserAsync(blockerId, blockedId);
                return Ok("User has been unblocked.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // "Block record not found."
            }
        }
    }
}