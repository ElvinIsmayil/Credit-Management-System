using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
