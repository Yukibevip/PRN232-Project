using Microsoft.AspNetCore.Mvc;

namespace PRN232_Project_MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult accusations()
        {
            return View();
        }

        public IActionResult friendlist()
        {
            return View();
        }

        public IActionResult logs()
        {
            return View();
        }

        public IActionResult users()
        {
            return View();
        }
    }
}
