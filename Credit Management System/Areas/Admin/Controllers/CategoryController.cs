using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Helpers;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseAdminController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CategoryCreateVM
                {
                    ParentCategories = await _categoryService.GetTopLevelCategoriesAsync()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData[AlertHelper.Error] = "Failed to initialize category form. Please try again.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await _categoryService.GetCreateViewModelAsync();
                    TempData[AlertHelper.Error] = "Please correct the form errors.";
                    return View(model);
                }

                var result = await _categoryService.AddAsync(model);
                if (result == null)
                {
                    // If result is null, it means either category creation failed OR image validation failed.
                    // The service now handles setting specific ModelState errors for image.
                    // We need to re-populate parent categories if returning the view.
                    await _categoryService.GetCreateViewModelAsync();

                    // If your CategoryService doesn't add specific ModelState errors,
                    // you might need to add a generic error here or rely on TempData from service.
                    // However, we need to ensure ModelState is populated correctly *if* image validation failed
                    // in the service and it returned null. This is crucial for user feedback.
                    // You might need a more sophisticated return type from service (e.g., Result pattern)
                    // to pass back validation errors from image service.
                    // For now, assuming CategoryService adds to ModelState or `TempData` is enough.
                    TempData[AlertHelper.Error] = "Failed to create category. Please check details and image file.";
                    return View(model);
                }

                TempData[AlertHelper.Success] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception) // Catching generic Exception for robustness
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while creating the category.";
                await _categoryService.GetCreateViewModelAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetUpdateByIdAsync(id);
            if (category == null)
            {
                TempData[AlertHelper.Error] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[AlertHelper.Error] = "Please correct the form errors.";
                    await _categoryService.GetUpdateByIdAsync(model.Id);
                    return View(model);
                }

                // *** Image handling is now done by CategoryService ***
                // The CategoryService.UpdateAsync will internally handle saving the new image,
                // deleting the old one, and updating the ImageUrl.

                var result = await _categoryService.UpdateAsync(model);
                if (result == null)
                {
                    // If result is null, it means either category update failed OR image validation failed.
                    // Similar to Create, if the service isn't populating ModelState, you might need more specific handling.
                    await _categoryService.GetUpdateByIdAsync(model.Id);
                    TempData[AlertHelper.Error] = "Failed to update category. Please check details and image file.";
                    return View(model);
                }

                TempData[AlertHelper.Success] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception) // Catching generic Exception for robustness
            {
                TempData[AlertHelper.Error] = "An unexpected error occurred while updating the category.";
                await _categoryService.GetUpdateByIdAsync(model.Id);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var categoryDetail = await _categoryService.GetDetailByIdAsync(id);
            if (categoryDetail == null)
            {
                TempData[AlertHelper.Error] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoryDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // The CategoryService.DeleteAsync will now handle getting the image URL
                // and calling _imageService.DeleteImage internally.
                var result = await _categoryService.DeleteAsync(id);

                if (!result)
                {
                    // CategoryService.DeleteAsync returns false if not found or deletion failed
                    return Json(new { success = false, message = "Category not found or deletion failed." });
                }

                return Json(new { success = true, message = "Category deleted successfully." });
            }
            catch (Exception) // Catching generic Exception for robustness
            {
                return Json(new { success = false, message = "An error occurred while deleting the category." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return Json(new { success = false, message = "No categories selected for deletion." });
            }

            int deletedCount = 0;
            List<string> failedDeletions = new List<string>();

            foreach (var id in ids)
            {
                try
                {
                    // The CategoryService.DeleteAsync will now handle image deletion internally
                    var result = await _categoryService.DeleteAsync(id);
                    if (result)
                    {
                        deletedCount++;
                    }
                    else
                    {
                        failedDeletions.Add($"Failed to delete category with ID {id}. (Not found or internal service error)");
                    }
                }
                catch (Exception ex)
                {
                    failedDeletions.Add($"Error deleting category with ID {id}: {ex.Message}");
                }
            }

            if (deletedCount == ids.Count)
            {
                return Json(new { success = true, message = $"{deletedCount} category(s) deleted successfully." });
            }
            else if (deletedCount > 0)
            {
                return Json(new { success = true, message = $"{deletedCount} category(s) deleted successfully. Some failed: {string.Join("; ", failedDeletions)}" });
            }
            else
            {
                return Json(new { success = false, message = $"Failed to delete any categories. Errors: {string.Join("; ", failedDeletions)}" });
            }
        }

       
    }
}