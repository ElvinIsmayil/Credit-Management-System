using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoanController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
