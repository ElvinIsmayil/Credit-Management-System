using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Infrastructure.Interfaces;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private const string ImageFolder = "products";
        public ProductController(IProductService productService, IImageService imageService)
        {
            _productService = productService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _productService.GetAllAsync();
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
        public IActionResult Create(ProductCreateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var data = _productService.AddAsync(model);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create product.");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productService.GetUpdateByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);
                var data = await _productService.UpdateAsync(model);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update product.");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetDetailByIdAsync(id);
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
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found." });
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    _imageService.DeleteImage(product.ImageUrl);
                }

                var result = await _productService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });

                return Json(new { success = true, message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the product: " + ex.Message });
            }
        }



    }
}
