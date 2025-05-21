using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Merchant;
using Credit_Management_System.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public ProductController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]   
        public IActionResult Index()
        {
            var service = GetProductService<ProductVM>();
            var products = service.GetAllAsync();
            if (products == null)
                return NotFound();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateVM productCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(productCreateVM);
                var service = GetProductService<ProductCreateVM>();
                var data = service.AddAsync(productCreateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create product.");
                    return View(productCreateVM);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(productCreateVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var service = GetProductService<ProductUpdateVM>();
            var product = await service.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateVM productUpdateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(productUpdateVM);
                var service = GetProductService<ProductUpdateVM>();
                var data = await service.UpdateAsync(productUpdateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update product.");
                    return View(productUpdateVM);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(productUpdateVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var service = GetProductService<ProductDetailVM>();
            var product = await service.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = GetProductService<ProductVM>();
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


        private IGenericService<TViewModel, Product> GetProductService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Product>>();
        }
    }
}
