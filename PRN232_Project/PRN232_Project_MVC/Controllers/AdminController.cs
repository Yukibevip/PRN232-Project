using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Services.Interfaces;
using System.Linq;
using PRN232_Project_MVC.Models;

namespace PRN232_Project_MVC.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ServicesMVC.Interfaces.IAccusationService _accusationService;
        private readonly ServicesMVC.Interfaces.IFriendListService _friendListService;
        private readonly ServicesMVC.Interfaces.IFriendInvitationService _friendInvitationServices;
        private readonly ServicesMVC.Interfaces.IBlockListService _blockListServices;
        private readonly ServicesMVC.Interfaces.IMessageService _messageServices;
        private readonly ServicesMVC.Interfaces.ILogService _logServices;

        public AdminController(IAdminService adminService, 
                                ServicesMVC.Interfaces.IAccusationService accusationService, 
                                ServicesMVC.Interfaces.IFriendListService friendListService, 
                                ServicesMVC.Interfaces.IFriendInvitationService friendInvitationService, 
                                ServicesMVC.Interfaces.IBlockListService blockListService,
                                ServicesMVC.Interfaces.IMessageService messageService,
                                ServicesMVC.Interfaces.ILogService logServices)
        {
            _adminService = adminService;
            _accusationService = accusationService;
            _friendListService = friendListService;
            _friendInvitationServices = friendInvitationService;
            _blockListServices = blockListService;
            _messageServices = messageService;
            _logServices = logServices;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Users"); // Chuyển hướng đến trang Users làm trang chính
        }

        [HttpGet("accusations")]
        public async Task<IActionResult> Accusations()
        {
            var accusations = await _accusationService.GetAll();
            return View(accusations);
        }

        [HttpPost("accusations/resolve/{id}")]
        public async Task<IActionResult> ResolveAccusation(int id)
        {
            var success = await _accusationService.ResolveAccusation(id);
            if (success)
            {
                TempData["Success"] = "Accusation resolved successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to resolve accusation.";
            }
            return RedirectToAction("Accusations");
        }

        [HttpGet("friendlist")]
        public async Task<IActionResult> Friendlist()
        {
            var friendlists = await _friendListService.GetFriendLists();
            var friendinvitations = await _friendInvitationServices.GetInvitations();
            var blocklists = await _blockListServices.GetBlockLists();
            var messages = await _messageServices.GetMessages();

            FriendListViewModel model = new FriendListViewModel
            {
                FriendLists = friendlists,
                FriendInvitations = friendinvitations,
                BlockLists = blocklists,
                Messages = messages
            };
            return View(model);
        }

        [HttpPost("friendlist/remove")]
        public async Task<IActionResult> DeleteFriendList(Guid user1Id, Guid user2Id)
        {
            var success = await _friendListService.RemoveFriendShip(user1Id, user2Id);
            if (success)
            {
                TempData["Success"] = "Friend list entry deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete friend list entry.";
            }
            return RedirectToAction("Friendlist");
        }

        [HttpPost("friendinvitation/remove")]
        public async Task<IActionResult> DeleteFriendInvitation(Guid user1Id, Guid user2Id)
        {
            var success = await _friendInvitationServices.RemoveFriendInvitation(user1Id, user2Id);
            if (success)
            {
                TempData["Success"] = "Friend list entry deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete friend list entry.";
            }
            return RedirectToAction("Friendlist");
        }

        [HttpGet("logs")]
        public async Task<IActionResult> LogsAsync()
        {
            var result = await _logServices.GetLogs();
            return View(result);
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users(string? q, string? status)
        {
            // Lấy danh sách người dùng dựa trên bộ lọc (q: tìm kiếm, status: trạng thái)
            var users = await _adminService.SearchUsersAsync(q, status);

            if (users == null)
            {
                users = new List<BusinessObjects.User>();
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
            var user = new BusinessObjects.User
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