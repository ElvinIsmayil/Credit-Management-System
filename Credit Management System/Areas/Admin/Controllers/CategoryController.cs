using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Extensions;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving categories.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Handle image upload
                if (model.Image != null)
                {
                    var validationErrors = model.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError(nameof(model.Image), error);
                        }
                        return View(model);
                    }

                    model.ImageUrl = await model.Image.SaveImageAsync(ImageFolder);
                }

                var result = await _categoryService.AddAsync(model);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create category.");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Category created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the category.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var category = await _categoryService.GetUpdateByIdAsync(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new CategoryUpdateVM
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    ImageUrl = category.ImageUrl,
                    ParentCategoryId = category.ParentCategoryId
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the category.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Handle image update
                if (model.Image != null)
                {
                    var validationErrors = model.Image.ValidateFileType().ToList();
                    if (validationErrors.Any())
                    {
                        foreach (var error in validationErrors)
                        {
                            ModelState.AddModelError(nameof(model.Image), error);
                        }
                        return View(model);
                    }

                    // Delete old image if it exists and isn't a default image
                    if (!string.IsNullOrEmpty(model.ImageUrl))
                    {
                        model.ImageUrl.DeleteImageFromLocal();
                    }

                    model.ImageUrl = await model.Image.SaveImageAsync(ImageFolder);
                }
                else
                {
                    // Keep existing image if no new file was uploaded
                    model.ImageUrl = model.ImageUrl;
                }

                var result = await _categoryService.UpdateAsync(model);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update category.");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the category.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var categoryDetail = await _categoryService.GetDetailByIdAsync(id);
                if (categoryDetail == null)
                {
                    TempData["ErrorMessage"] = "Category not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(categoryDetail);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the category details.";
                return RedirectToAction(nameof(Index));
            }
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
                if (!result)
                {
                    return Json(new { success = false, message = "Failed to delete category." });
                }

                return Json(new { success = true, message = "Category deleted successfully!" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while deleting the category." });
            }
        }



    }
}
