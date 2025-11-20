using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces; // Đảm bảo bạn đang sử dụng Interface
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN232_Project_MVC.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly APIService _apiService;
        public AdminController(IAdminService adminService, APIService aPIService) // Sử dụng Interface
        {
            _adminService = adminService;
            _apiService = aPIService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Users"); // Chuyển hướng đến trang Users làm trang chính
        }

        public IActionResult Accusations()
        {
            return View();
        }

        public IActionResult Friendlist()
        {
            return View();
        }

        public IActionResult Logs()
        {
            return View();
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users(string? q, string? status)
        {
            // Lấy danh sách người dùng dựa trên bộ lọc (q: tìm kiếm, status: trạng thái)
            var users = await _adminService.SearchUsersAsync(q, status);

            if (users == null)
            {
                users = new List<User>();
                ViewData["Error"] = "Không thể tải danh sách người dùng.";
            }

            ViewData["CurrentFilter"] = q;
            ViewData["CurrentStatus"] = status ?? "";

            return View(users); // Truyền danh sách (Model) vào View
        }

        [HttpPost("AddUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(string username, string email, string fullName, string password, string gender, string userRole)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = username ?? string.Empty,
                Password = password ?? string.Empty, // Nhớ HASH mật khẩu này trong service!
                Email = email ?? string.Empty,
                FullName = fullName ?? string.Empty,
                Gender = gender ?? string.Empty,
                UserRole = userRole ?? "User", // Lấy từ form (Admin/User)
                Status = "Active" // Mặc định khi tạo mới là "Active"
            };

            var ok = await _adminService.CreateUserAsync(user);
            if (!ok)
            {
                TempData["Error"] = "Không thể tạo người dùng. Tên đăng nhập hoặc Email có thể đã tồn tại.";
            }
            else
            {
                TempData["Success"] = "Người dùng đã được tạo thành công.";
            }

            return RedirectToAction("users");
        }
        // Trong AdminController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(User user)
        {

            var existingUser = await _apiService.GetUserByIdAsync(user.UserId);
            if (existingUser != null)
            {
                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Gender = user.Gender;
                existingUser.UserRole = user.UserRole;

                var result = await _adminService.UpdateUserByAdminAsync(existingUser);
                if (result)
                {
                    TempData["Success"] = "User updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to update user.";
                }
            }

            return RedirectToAction("Users");
        }
        // POST: /Admin/ChangeStatus
        // Action này xử lý cả "Active" và "Suspended"
        [HttpPost("ChangeStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(Guid id, string status)
        {
            var ok = await _adminService.ChangeUserStatusAsync(id, status);
            if (!ok)
            {
                TempData["Error"] = "Không thể thay đổi trạng thái.";
            }
            else
            {
                TempData["Success"] = "Trạng thái đã được cập nhật.";
            }
            return RedirectToAction("users");
        }

        // POST: /Admin/DeleteUser
        [HttpPost("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var ok = await _adminService.DeleteUserAsync(id);
            if (!ok)
            {
                TempData["Error"] = "Không thể xóa người dùng.";
            }
            else
            {
                TempData["Success"] = "Người dùng đã được xóa.";
            }
            return RedirectToAction("users");
        }

        // POST: /Admin/BlockUser
        // Action này xử lý "Banned"
        [HttpPost("BlockUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(Guid id)
        {
            // Chúng ta cần một phương thức riêng trong service để xử lý Banned
            // Hoặc chúng ta có thể gọi ChangeStatus với "Banned"
            // Giả sử IAdminService có BlockUserAsync như trong file của bạn
            var ok = await _adminService.BlockUserAsync(id);
            if (!ok)
            {
                TempData["Error"] = "Không thể chặn người dùng.";
            }
            else
            {
                TempData["Success"] = "Người dùng đã bị cấm.";
            }
            return RedirectToAction("users");
        }

        // GET: /Admin/ExportUsers
        [HttpGet("ExportUsers")]
        public async Task<IActionResult> ExportUsers(string? q, string? status)
        {
            var file = await _adminService.ExportUsersAsync(q, status);
            if (file == null)
            {
                TempData["Error"] = "Không có dữ liệu để xuất.";
                return RedirectToAction("users");
            }
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
        }
    }
}