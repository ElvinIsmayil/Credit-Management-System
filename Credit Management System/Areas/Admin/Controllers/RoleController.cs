using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
