using Credit_Management_System.Extensions;
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
            try
            {
                if (!ModelState.IsValid)
                    return View(merchantCreateVM);

                if (merchantCreateVM.Image != null)
                {
                    var validationErrors = merchantCreateVM.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError("Image", error);
                        }
                        return View(merchantCreateVM);
                    }
                    merchantCreateVM.ImageUrl = await merchantCreateVM.Image.SaveImageAsync("merchants");
                }

                var service = GetMerchantService<MerchantCreateVM>();
                var data = await service.AddAsync(merchantCreateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create merchant.");
                    return View(merchantCreateVM);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the merchant.");
                return View(merchantCreateVM);
            }
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
            try
            {
                if (!ModelState.IsValid)
                    return View(merchantUpdateVM);

                var service = GetMerchantService<MerchantUpdateVM>();
                if (merchantUpdateVM.Image != null)
                {
                    var validationErrors = merchantUpdateVM.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                            ModelState.AddModelError("Image", error);

                        return View(merchantUpdateVM);
                    }

                    if (!string.IsNullOrEmpty(merchantUpdateVM.ImageUrl))
                        merchantUpdateVM.ImageUrl.DeleteImageFromLocal();

                    merchantUpdateVM.ImageUrl = await merchantUpdateVM.Image.SaveImageAsync("merchants");
                }
                await service.UpdateAsync(merchantUpdateVM);
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the merchant.");
                return View(merchantUpdateVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = GetMerchantService<MerchantVM>();
                var result = await service.DeleteAsync(id);

                if (!result)
                {
                    return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });
                }

                return Json(new { success = true, message = "Item deleted successfully." });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the item." });
            }
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
