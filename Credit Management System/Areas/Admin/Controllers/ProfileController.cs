using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : BaseAdminController
    {
        private readonly IUserService _userService;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

    }
}
