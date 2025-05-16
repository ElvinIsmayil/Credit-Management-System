using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MerchantController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public MerchantController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var service = GetMerchantService<MerchantVM>();
            var merchants = await service.GetAllAsync();
            return View(merchants);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MerchantCreateVM merchantCreateVM)
        {
            var service = GetMerchantService<MerchantCreateVM>();
            var data = await service.AddAsync(merchantCreateVM);
            if (data == null)
            {
                return View();
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {  
            var service = GetMerchantService<MerchantUpdateVM>();
            var merchant = await service.GetByIdAsync(id);
            return View(merchant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MerchantUpdateVM merchantUpdateVM)
        {
            var service = GetMerchantService<MerchantUpdateVM>();

            service.UpdateAsync(merchantUpdateVM);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = GetMerchantService<MerchantVM>();
            var result = await service.DeleteAsync(id);

            if (!result)
            {
                return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });
            }

            return Json(new { success = true, message = "Item deleted successfully." });
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var service = GetMerchantService<MerchantDetailVM>();
            var merchant = await service.GetByIdAsync(id);

            return View(merchant);

        }

        private IGenericService<TViewModel, Merchant> GetMerchantService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Merchant>>();
        }


    }
}
