using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Merchant;
using Credit_Management_System.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
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
        public IActionResult Create(ProductCreateVM productCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(productCreateVM);

                var data = _productService.AddAsync(productCreateVM);
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
            var product = await _productService.GetUpdateByIdAsync(id);
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
                var data = await _productService.UpdateAsync(productUpdateVM);
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
                var result = await _productService.DeleteAsync(id);

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


        
    }
}
