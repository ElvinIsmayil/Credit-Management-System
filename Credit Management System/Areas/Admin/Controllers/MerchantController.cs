using Credit_Management_System.Extensions;
using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;
        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var merchants = await _merchantService.GetAllAsync();
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

                var data = await _merchantService.AddAsync(merchantCreateVM);

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
            var merchant = await _merchantService.GetUpdateByIdAsync(id);
            if (merchant == null)
                return NotFound();

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
                await _merchantService.UpdateAsync(merchantUpdateVM);
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
                var result = await _merchantService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });

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
            var merchant = await _merchantService.GetDetailByIdAsync(id);
            return View(merchant);
        }
    }
}
