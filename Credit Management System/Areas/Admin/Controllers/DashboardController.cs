using Credit_Management_System.Areas.Admin.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : BaseAdminController
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
