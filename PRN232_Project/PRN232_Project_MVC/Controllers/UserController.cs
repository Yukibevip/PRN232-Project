using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly APIService _apiService;
        public UserController(APIService apiService) => _apiService = apiService;

        public IActionResult calling()
        {
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        public async Task<IActionResult> setting()
        {
            // try get user id from session
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToAction("login");
            }

            Guid userId = Guid.Empty;
            try
            {
                using var doc = JsonDocument.Parse(userJson);
                if (doc.RootElement.TryGetProperty("UserId", out var idEl) && idEl.ValueKind == JsonValueKind.String)
                {
                    Guid.TryParse(idEl.GetString(), out userId);
                }
            }
            catch { }

            if (userId == Guid.Empty)
            {
                // fallback: show view without full data
                return View();
            }

            var user = await _apiService.GetUserByIdAsync(userId);
            ViewBag.User = user;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(IFormFile? avatar, string? fullname, string? email, string? gender)
        {
            // get user id from session
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson)) return RedirectToAction("login");

            Guid userId = Guid.Empty;
            try
            {
                using var doc = JsonDocument.Parse(userJson);
                if (doc.RootElement.TryGetProperty("UserId", out var idEl) && idEl.ValueKind == JsonValueKind.String)
                {
                    Guid.TryParse(idEl.GetString(), out userId);
                }
            }
            catch { }

            if (userId == Guid.Empty) return RedirectToAction("login");

            string? avatarUrl = null;
            if (avatar != null && avatar.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(avatar.FileName)}";
                var filePath = Path.Combine(uploads, fileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }
                avatarUrl = $"/avatars/{fileName}";
            }

            // call API to update DB (include gender)
            var updated = await _apiService.UpdateUserProfileAsync(userId, fullname, email, avatarUrl, gender);

            if (updated)
            {
                // refresh session minimal info
                var user = await _apiService.GetUserByIdAsync(userId);
                var sessionUser = new { user.UserId, user.Username, user.FullName, user.Gender, user.UserRole };
                HttpContext.Session.SetString("User", JsonSerializer.Serialize(sessionUser));

                ViewBag.Message = "Profile updated successfully.";
            }
            else
            {
                ViewBag.Error = "Failed to update profile.";
            }

            // show updated profile values
            ViewBag.User = await _apiService.GetUserByIdAsync(userId);
            return View("setting");
        }
        public IActionResult block()
        {
            return View();
        }

        public IActionResult friend()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _apiService.LoginAsync(username, password);
            if (user == null)
            {
                ViewBag.LoginError = "Login failed: invalid username or password.";
                return View("login");
            }

            // store minimal user info in session (JSON) and include role
            var sessionUser = new { user.UserId, user.Username, user.FullName, user.UserRole };
            HttpContext.Session.SetString("User", JsonSerializer.Serialize(sessionUser));

            // Redirect admin to admin users page, regular users to home as before
            if (!string.IsNullOrEmpty(user.UserRole) && user.UserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("users", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string? googleId)
        {
            var success = await _apiService.RegisterAsync(username, email, password, googleId);
            if (!success)
            {
                ViewBag.RegisterError = "Register failed: username or email may already exist.";
                return View("register");
            }
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Please enter your email.";
                return View();
            }

            var exists = await _apiService.CheckEmailExistsAsync(email);
            if (!exists)
            {
                ViewBag.Error = "Email is not registered.";
                return View();
            }

            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("ResetEmail", email);
            HttpContext.Session.SetString("ResetOTP", otp);
            HttpContext.Session.SetString("ResetOtpExpiry", DateTime.UtcNow.AddMinutes(10).ToString("o"));

            SendOtpEmail(email, otp);

            TempData["Email"] = email;
            return RedirectToAction("VerifyOtp");
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            ViewBag.Email = TempData["Email"] ?? HttpContext.Session.GetString("ResetEmail");
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOtp(string email, string otp)
        {
            var sessionOtp = HttpContext.Session.GetString("ResetOTP");
            var sessionEmail = HttpContext.Session.GetString("ResetEmail");
            var expiryStr = HttpContext.Session.GetString("ResetOtpExpiry");
            DateTime expiry = DateTime.MinValue;
            if (!string.IsNullOrEmpty(expiryStr)) DateTime.TryParse(expiryStr, out expiry);

            if (sessionOtp == null || sessionEmail == null || expiry < DateTime.UtcNow)
            {
                ViewBag.Error = "OTP expired or not found. Please request a new OTP.";
                return RedirectToAction("ForgotPassword");
            }

            if (sessionOtp == otp && sessionEmail == email)
            {
                TempData["Email"] = email;
                // Đánh dấu đã xác thực OTP
                HttpContext.Session.SetString("OtpVerified", "true");
                return RedirectToAction("ResetPassword");
            }
            else
            {
                ViewBag.Error = "Invalid OTP or email.";
                ViewBag.Email = email;
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            // Chỉ cho phép nếu đã xác thực OTP
            if (HttpContext.Session.GetString("OtpVerified") != "true")
                return RedirectToAction("ForgotPassword");

            ViewBag.Email = TempData["Email"] ?? HttpContext.Session.GetString("ResetEmail");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email, string newPassword)
        {
            if (HttpContext.Session.GetString("OtpVerified") != "true")
                return RedirectToAction("ForgotPassword");

            var ok = await _apiService.ResetPasswordByEmailAsync(email, newPassword);
            if (ok)
            {
                HttpContext.Session.Remove("ResetOTP");
                HttpContext.Session.Remove("ResetEmail");
                HttpContext.Session.Remove("ResetOtpExpiry");
                HttpContext.Session.Remove("OtpVerified");
                ViewBag.Message = "Password has been reset. Please login.";
                return RedirectToAction("login");
            }
            else
            {
                ViewBag.Error = "Failed to reset password. Try again later.";
                ViewBag.Email = email;
                return View();
            }
        }

        private void SendOtpEmail(string email, string otp)
        {
            // Read SMTP settings from configuration in production. This example uses hard-coded SMTP.\string appPassword = Environment.GetEnvironmentVariable("GMAIL_APP_PASSWORD");
            string appPassword = Environment.GetEnvironmentVariable("GMAIL_APP_PASSWORD");
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tai2052004@gmail.com", appPassword),
                EnableSsl = true,
            };
            smtp.Send("tai2052004@gmail.com", email, "Your OTP Code", $"Your OTP is: {otp}");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}