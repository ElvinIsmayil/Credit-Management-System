using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Category;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryCreateVM categoryCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(categoryCreateVM);
                var service = GetCategoryService<CategoryCreateVM>();
                var data = service.AddAsync(categoryCreateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create category.");
                    return View(categoryCreateVM);
                }
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoryCreateVM);
            }


        }

        private IGenericService<TViewModel, Category> GetCategoryService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Category>>();
        }
    }
}
