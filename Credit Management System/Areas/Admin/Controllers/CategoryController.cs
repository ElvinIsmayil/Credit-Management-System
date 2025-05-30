using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Extensions;
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
        private const string ImageFolder = "categories";

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
            if (!ModelState.IsValid)
            {
                await PopulateParentCategories(model.ParentCategoryId);
                return View(model);
            }

            try
            {
                if (model.Image != null)
                {
                    var validationErrors = model.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError("Image", error);
                        }
                        return View(model);
                    }
                    model.ImageUrl = await model.Image.SaveImageAsync(ImageFolder);
                }

                var result = await _categoryService.AddAsync(model);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create category.");
                    await PopulateParentCategories(model.ParentCategoryId);
                    return View(model);
                }

                TempData[AlertHelper.Success] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData[AlertHelper.Error] = "An error occurred while creating the category.";
                await PopulateParentCategories(model.ParentCategoryId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.GetUpdateByIdAsync(id);
            if (category == null)
            {
                TempData[AlertHelper.Success] = "Category not found.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateParentCategories(category.ParentCategoryId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateParentCategories(model.ParentCategoryId);
                return View(model);
            }

            try
            {
                if (model.Image != null)
                {
                    var validationErrors = model.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                            ModelState.AddModelError("Image", error);

                        return View(model);
                    }

                    if (!string.IsNullOrEmpty(model.ImageUrl))
                        model.ImageUrl.DeleteImageFromLocal();

                    model.ImageUrl = await model.Image.SaveImageAsync(ImageFolder);
                }

                var result = await _categoryService.UpdateAsync(model);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update category.");
                    await PopulateParentCategories(model.ParentCategoryId);
                    return View(model);
                }

                TempData[AlertHelper.Success] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData[AlertHelper.Error] = "An error occurred while updating the category.";
                await PopulateParentCategories(model.ParentCategoryId);
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
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    category.ImageUrl.DeleteImageFromLocal();
                }

                var result = await _categoryService.DeleteAsync(id);
                return Json(new
                {
                    success = result,
                    message = result ? "Category deleted successfully!" : "Failed to delete category."
                });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the category." });
            }
        }

        #region Helper Methods
        private async Task PopulateParentCategories(int? selectedId = null)
        {
            var parentCategories = await _categoryService.GetTopLevelCategoriesAsync();
            ViewBag.ParentCategoryId = new SelectList(parentCategories, "Id", "Name", selectedId);
        }
        #endregion

    }
}