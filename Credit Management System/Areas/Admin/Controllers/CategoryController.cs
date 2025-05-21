using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels;
using Credit_Management_System.ViewModels.Category;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public CategoryController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Index - List of Categories
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var service = GetCategoryService<CategoryVM>();
            var categories = await service.GetAllAsync();
            return View(categories);
        }

        // Create - Show the Create Form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch parent categories to display in the dropdown
            var service = GetCategoryService<CategoryCreateVM>();
            var parentCategories = await service.GetParentCategoriesAsync(); // assuming you have a method to fetch parent categories

            var categoryCreateVM = new CategoryCreateVM
            {
                ParentCategories = parentCategories
            };

            return View(categoryCreateVM);
        }

        // Create - Handle the Post request for creating a category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM categoryCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(categoryCreateVM);

                // Ensure SubCategories is not null before mapping
                if (categoryCreateVM.SubCategories == null)
                {
                    categoryCreateVM.SubCategories = new List<SubCategoryVM>();
                }

                // Add the category
                var service = GetCategoryService<CategoryCreateVM>();
                var data = await service.AddAsync(categoryCreateVM);

                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create category.");
                    return View(categoryCreateVM);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoryCreateVM);
            }
        }

        // Update - Show the Update Form
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var service = GetCategoryService<CategoryUpdateVM>();
            var category = await service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            // Fetch parent categories to display in the dropdown
            var parentCategoriesService = GetCategoryService<CategoryCreateVM>();
            var parentCategories = await parentCategoriesService.GetParentCategoriesAsync();

            category.ParentCategories = parentCategories;

            return View(category);
        }

        // Update - Handle the Post request for updating the category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM categoryUpdateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(categoryUpdateVM);

                var service = GetCategoryService<CategoryUpdateVM>();
                var data = await service.UpdateAsync(categoryUpdateVM);

                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update category.");
                    return View(categoryUpdateVM);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoryUpdateVM);
            }
        }

        // Detail - Show details of a category
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var service = GetCategoryService<CategoryDetailVM>();
            var category = await service.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        // Delete - Handle category deletion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = GetCategoryService<MerchantVM>();
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

        // Helper method to get the appropriate service
        private IGenericService<TViewModel, Category> GetCategoryService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Category>>();
        }
    }
}
