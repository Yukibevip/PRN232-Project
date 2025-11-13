using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using Services.Interfaces;
using Services;
using AutoMapper;
using BusinessObjects.Dto;

namespace PRN232_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _adminService;
        private readonly IAccusationService _accusationService;
        private readonly IMapper _mapper;

        public AdminController(IUserService adminService, IAccusationService accusationService, IMapper mapper)
        {
            _adminService = adminService;
            _accusationService = accusationService;
            _mapper = mapper;
        }

        // GET: api/admin/users
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return users is null ? NotFound() : Ok(users);
        }

        // GET: api/admin/users/search?q=...&status=...
        [HttpGet("users/search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers([FromQuery] string? q, [FromQuery] string? status)
        {
            var users = await _adminService.SearchUsersAsync(q, status);
            return users is null ? NotFound() : Ok(users);
        }

        // POST: api/admin/users
        [HttpPost("users")]
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            if (user == null) return BadRequest();

            if (user.UserId == Guid.Empty) user.UserId = Guid.NewGuid();

            var ok = await _adminService.CreateUserAsync(user);
            if (!ok)
            {
                // service returns false for failure (could be duplicate or other validation)
                return BadRequest(new { error = "Failed to create user (possible duplicate or validation error)." });
            }

            // Return created resource. Attempt to fetch fresh data from service list (best-effort).
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // GET: api/admin/users/{id}
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var users = await _adminService.GetUsersAsync();
            if (users == null) return NotFound();
            var user = users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/admin/users/change-status/{id}
        [HttpPost("users/change-status/{id}")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] Dictionary<string, string> data)
        {
            if (!data.TryGetValue("status", out var status) || string.IsNullOrWhiteSpace(status))
                return BadRequest(new { error = "status is required" });

            var ok = await _adminService.ChangeUserStatusAsync(id, status);
            if (!ok) return NotFound(new { error = "user not found or status not updated" });

            return Ok(new { message = "status updated" });
        }

        // POST: api/admin/users/block/{id}
        [HttpPost("users/block/{id}")]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            var ok = await _adminService.BlockUserAsync(id);
            if (!ok) return NotFound(new { error = "user not found or block failed" });

            return Ok(new { message = "user banned" });
        }

        // DELETE: api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var ok = await _adminService.DeleteUserAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }

        // GET: api/admin/users/export?q=...&status=...
        [HttpGet("users/export")]
        public async Task<IActionResult> Export([FromQuery] string? q, [FromQuery] string? status)
        {
            var bytes = await _adminService.ExportUsersAsync(q, status);
            if (bytes == null || bytes.Length == 0) return NotFound();

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
        }

        [HttpGet("accusations")]
        public async Task<ActionResult<IEnumerable<Accusation>>> GetAccusations()
        {
            var accusations = await _accusationService.GetAll();
            var dtos = _mapper.Map<IEnumerable<AccusationDto>>(accusations);
            return dtos is null ? NotFound() : Ok(dtos);
        }

        [HttpGet("accusations/{id}")]
        public async Task<ActionResult<Accusation>> GetAccusation(int id)
        {
            var accusation = await _accusationService.Get(id);
            return accusation is null ? NotFound() : Ok(accusation);
        }

        [HttpPost("accusations")]
        public async Task<ActionResult<Accusation>> CreateAccusation([FromBody] Accusation accusation)
        {
            if (accusation == null) return BadRequest();
            var ok = await _accusationService.Add(accusation);
            if (!ok) return BadRequest(new { error = "Failed to create accusation." });
            return CreatedAtAction(nameof(GetAccusation), new { id = accusation.AccusationId }, accusation);
        }

        [HttpPut("accusations/{id}")]
        public async Task<IActionResult> UpdateAccusation(int id, [FromBody] Accusation accusation)
        {
            if (accusation == null || accusation.AccusationId != id) return BadRequest();
            var ok = await _accusationService.Update(accusation);
            if (!ok) return NotFound(new { error = "Accusation not found or update failed." });
            return Ok();
        }

        [HttpDelete("accusations/{id}")]
        public async Task<IActionResult> DeleteAccusation(int id)
        {
            var ok = await _accusationService.Delete(id);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
