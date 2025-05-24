using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoanController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        public IActionResult Index()
        {
            return View();
        }

        private IGenericService<TViewModel, Merchant> GetMerchantService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Merchant>>();
        }
    }
}
