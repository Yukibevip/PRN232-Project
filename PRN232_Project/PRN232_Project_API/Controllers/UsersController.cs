using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Services.Interfaces;

namespace PRN232_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CallioTestContext _context;
        private readonly IUserService _userService;

        public UsersController(CallioTestContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return user;
        }

        // --- Authentication / account endpoints (use IUserService) ---

        // POST: api/Users/login
        // Body JSON example: { "Username": "alice", "Password": "pass123" }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("username", out var username);
            lookup.TryGetValue("password", out var password);

            Console.WriteLine($"Login attempt: {username} / {password}");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { error = "username and password are required" });

            var user = _userService.Login(username, password);
            if (user == null)
            {
                Console.WriteLine("Login failed: invalid credentials");
                return Unauthorized(new { error = "invalid credentials" });
            }
                

            user.Password = null!;
            return Ok(user);
        }

        // POST: api/Users/login-google
        // Body JSON example: { "Email": "user@example.com", "FullName": "User Name" }
        [HttpPost("login-google")]
        public IActionResult LoginWithGoogle([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("email", out var email);
            lookup.TryGetValue("fullname", out var fullName);

            if (string.IsNullOrWhiteSpace(email))
                return BadRequest(new { error = "email is required" });

            var user = _userService.GetUserByUsername(email); // try lookup by email as username
            if (user != null)
            {
                user.Password = null!;
                return Ok(user);
            }

            // create minimal user via service/repo layer
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = email,
                Email = email,
                FullName = fullName ?? string.Empty,
                Password = string.Empty,
                UserRole = "User"
            };

            // use repository via service.Register
            _userService.Register(newUser);
            newUser.Password = null!;
            return CreatedAtAction(nameof(GetUser), new { id = newUser.UserId }, newUser);
        }

        // POST: api/Users/register
        // Body JSON example: { "Username":"alice","Password":"pass","Email":"a@b.com","FullName":"Alice" }
        [HttpPost("register")]
        public IActionResult Register([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("username", out var username);
            lookup.TryGetValue("password", out var password);
            lookup.TryGetValue("email", out var email);
            lookup.TryGetValue("fullname", out var fullName);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return BadRequest(new { error = "username and password are required" });

            // duplication checks using context for now
            if (_context.Users.Any(u => u.Username == username))
                return Conflict(new { error = "username already exists" });

            if (!string.IsNullOrWhiteSpace(email) && _context.Users.Any(u => u.Email == email))
                return Conflict(new { error = "email already exists" });

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                Password = password,
                Email = email ?? string.Empty,
                FullName = fullName ?? string.Empty,
                UserRole = "User"
            };

            // delegate creation to service
            _userService.Register(user);

            user.Password = null!;
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // POST: api/Users/forgot-password
        // Body JSON example: { "username": "alice" }
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("username", out var username);

            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { error = "username is required" });

            var user = _userService.GetUserByUsername(username);
            if (user == null)
                return NotFound(new { error = "user not found" });

            // production: generate token and send email. Here indicate flow started.
            return Ok(new { message = "If the account exists, password reset instructions have been sent." });
        }

        // POST: api/Users/check-email
        // Body JSON example: { "email": "user@example.com" } or { "username":"alice" }
        [HttpPost("check-email")]
        public IActionResult CheckEmail([FromBody] Dictionary<string, string> data)
        {
            if (data == null || !data.TryGetValue("email", out var email) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { error = "Email is required" });
            }

            bool exists = _context.Users.Any(u => u.Email == email);

            if (!exists)
                return NotFound(new { error = "User not found" });

            return Ok(new { message = "User exists" });
        }


        // POST: api/Users/reset-password
        // Accepts either { "userId": "<guid>", "newPassword":"..." } OR { "email": "user@example.com", "newPassword":"..." }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("userId", out var userIdStr);
            lookup.TryGetValue("email", out var email);
            lookup.TryGetValue("newPassword", out var newPassword);

            if (string.IsNullOrWhiteSpace(newPassword))
                return BadRequest(new { error = "newPassword is required" });

            User? user = null;
            if (!string.IsNullOrWhiteSpace(userIdStr) && Guid.TryParse(userIdStr, out var userId))
            {
                user = await _context.Users.FindAsync(userId);
            }
            else if (!string.IsNullOrWhiteSpace(email))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }

            if (user == null)
                return NotFound(new { error = "user not found" });

            user.Password = newPassword;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "password updated" });
        }

        // POST: api/Users/update-profile
        // Body JSON example: { "UserId": "<guid>", "FullName":"...", "Email":"...", "AvatarUrl":"/avatars/..." }
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] Dictionary<string, string> data)
        {
            var lookup = data == null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);

            lookup.TryGetValue("userId", out var userIdStr);
            lookup.TryGetValue("fullname", out var fullName);
            lookup.TryGetValue("email", out var email);
            lookup.TryGetValue("avatarurl", out var avatarUrl);
            lookup.TryGetValue("gender", out var gender);

            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var userId))
                return BadRequest(new { error = "invalid userId" });

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound(new { error = "user not found" });

            if (!string.IsNullOrWhiteSpace(fullName)) user.FullName = fullName;
            if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
            if (!string.IsNullOrWhiteSpace(avatarUrl)) user.AvatarUrl = avatarUrl;
            if (!string.IsNullOrWhiteSpace(gender)) user.Gender = gender;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "profile updated" });
        }

        // existing CRUD endpoints below (unchanged) ...
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}