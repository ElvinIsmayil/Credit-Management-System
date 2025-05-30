using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : BaseAdminController
    {
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            var fullName = user != null ? $"{user.FirstName} {user.LastName}" : User.Identity.Name;
            ViewData["FullName"] = fullName;

            return View();
        }
    }
}
