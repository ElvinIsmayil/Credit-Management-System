using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Helpers;
using Credit_Management_System.Infrastructure.Interfaces;
using Credit_Management_System.Services.Interfaces; // Assuming IImageService is here or in a new Infrastructure.Services.Interfaces
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Mvc;
using System; // Added for Exception

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MerchantController : BaseAdminController
    {
        private readonly IMerchantService _merchantService;
        private readonly IImageService _imageService; // Inject the new image service

        public MerchantController(IMerchantService merchantService, IImageService imageService)
        {
            _merchantService = merchantService;
            _imageService = imageService; // Initialize the image service
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm; 
            if(!string.IsNullOrEmpty(searchTerm))
            {
                var filteredMerchants = await _merchantService.SearchMerchantAsync(searchTerm);
                return View(filteredMerchants);
            }
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
                {
                    TempData[AlertHelper.Error] = "Please correct the form errors.";
                    TempData["AlertType"] = "toastr";
                    return View(merchantCreateVM);
                }

                if (merchantCreateVM.Image != null)
                {
                    // Use the ImageService for validation and saving
                    var (imageUrl, validationErrors) = await _imageService.SaveImageAsync(merchantCreateVM.Image, "merchants");

                    if (validationErrors != null && validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError("Image", error);
                        }
                        TempData[AlertHelper.Error] = "Invalid image file type. Please upload a valid image (e.g., JPG, PNG).";
                        TempData["AlertType"] = "toastr";
                        return View(merchantCreateVM);
                    }
                    merchantCreateVM.ImageUrl = imageUrl;
                }

                var data = await _merchantService.AddAsync(merchantCreateVM);

                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create merchant.");
                    TempData[AlertHelper.Error] = "Failed to create merchant. Please try again.";
                    TempData["AlertType"] = "toastr";
                    return View(merchantCreateVM);
                }

                TempData[AlertHelper.Success] = "Merchant created successfully!";
                TempData["AlertType"] = "toastr";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) // Catch specific exceptions or log them
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the merchant: " + ex.Message);
                TempData[AlertHelper.Error] = "An unexpected error occurred while creating the merchant. Please try again later.";
                TempData["AlertType"] = "toastr";
                return View(merchantCreateVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var merchant = await _merchantService.GetUpdateByIdAsync(id);
            if (merchant == null)
            {
                TempData[AlertHelper.Error] = "Merchant not found.";
                TempData["AlertType"] = "toastr";
                return NotFound();
            }
            return View(merchant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(MerchantUpdateVM merchantUpdateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Please correct the form errors.";
                    TempData["AlertType"] = "toastr";
                    return View(merchantUpdateVM);
                }

                var existingMerchant = await _merchantService.GetByIdAsync(merchantUpdateVM.Id);
                if (existingMerchant == null)
                {
                    TempData[AlertHelper.Error] = "Merchant not found for update.";
                    TempData["AlertType"] = "toastr";
                    return NotFound();
                }

                string currentImageUrl = existingMerchant.ImageUrl;

                if (merchantUpdateVM.Image != null)
                {
                    // Use the ImageService for validation, deletion of old, and saving new
                    var (newImageUrl, validationErrors) = await _imageService.SaveImageAsync(merchantUpdateVM.Image, "merchants", currentImageUrl);

                    if (validationErrors != null && validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                            ModelState.AddModelError("Image", error);

                        TempData[AlertHelper.Error] = "Invalid image file type. Please upload a valid image (e.g., JPG, PNG).";
                        TempData["AlertType"] = "toastr";
                        return View(merchantUpdateVM);
                    }
                    merchantUpdateVM.ImageUrl = newImageUrl;
                }
                else
                {
                    // If no new image is provided, retain the existing one
                    merchantUpdateVM.ImageUrl = currentImageUrl;
                }

                await _merchantService.UpdateAsync(merchantUpdateVM);
                TempData[AlertHelper.Success] = "Merchant updated successfully!";
                TempData["AlertType"] = "toastr";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the merchant: " + ex.Message);
                TempData[AlertHelper.Error] = "An unexpected error occurred while updating the merchant. Please try again later.";
                TempData["AlertType"] = "toastr";
                return View(merchantUpdateVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var merchant = await _merchantService.GetByIdAsync(id);
                if (merchant == null)
                {
                    return Json(new { success = false, message = "Merchant not found." });
                }

                // Use the ImageService to delete the image
                if (!string.IsNullOrEmpty(merchant.ImageUrl))
                {
                    _imageService.DeleteImage(merchant.ImageUrl);
                }

                var result = await _merchantService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });

                return Json(new { success = true, message = "Merchant deleted successfully." });
            }
            catch (Exception ex) // Catch specific exceptions or log them
            {
                return Json(new { success = false, message = "An error occurred while deleting the merchant: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var merchant = await _merchantService.GetDetailByIdAsync(id);
            if (merchant == null)
            {
                TempData[AlertHelper.Error] = "Merchant details not found.";
                TempData["AlertType"] = "toastr";
                return NotFound();
            }
            return View(merchant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No merchants selected for deletion." });
            }

            int deletedCount = 0;
            List<string> failedDeletions = new List<string>();

            foreach (var id in ids)
            {
                try
                {
                    var merchant = await _merchantService.GetByIdAsync(id);
                    if (merchant == null)
                    {
                        failedDeletions.Add($"Merchant with ID {id} not found.");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(merchant.ImageUrl))
                    {
                        _imageService.DeleteImage(merchant.ImageUrl);
                    }

                    var result = await _merchantService.DeleteAsync(id);
                    if (result)
                    {
                        deletedCount++;
                    }
                    else
                    {
                        failedDeletions.Add($"Failed to delete merchant with ID {id}.");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception for each failed deletion
                    failedDeletions.Add($"Error deleting merchant with ID {id}: {ex.Message}");
                }
            }

            if (deletedCount == ids.Count)
            {
                return Json(new { success = true, message = $"{deletedCount} merchant(s) deleted successfully." });
            }
            else if (deletedCount > 0)
            {
                return Json(new { success = true, message = $"{deletedCount} merchant(s) deleted successfully. Some failed: {string.Join("; ", failedDeletions)}" });
            }
            else
            {
                return Json(new { success = false, message = $"Failed to delete any merchants. Errors: {string.Join("; ", failedDeletions)}" });
            }
        }
    }
}
