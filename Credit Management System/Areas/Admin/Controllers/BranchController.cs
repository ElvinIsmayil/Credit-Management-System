using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.ViewModels.Branch;
using Credit_Management_System.ViewModels.Merchant;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        public BranchController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var service = GetBranchService<Merchant>();
            var branches = service.GetAllAsync();
            return View(branches);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchCreateVM branchCreateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branchCreateVM);
                var service = GetBranchService<BranchCreateVM>();
                var data = service.AddAsync(branchCreateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create branch.");
                    return View(branchCreateVM);
                }
                return RedirectToAction("Index", "Branch", new { area = "Admin" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(branchCreateVM);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var service = GetBranchService<BranchUpdateVM>();
            var branch = await service.GetByIdAsync(id);
            return View(branch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BranchUpdateVM branchUpdateVM)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(branchUpdateVM);
                var service = GetBranchService<BranchUpdateVM>();
                var data = await service.UpdateAsync(branchUpdateVM);
                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update branch.");
                    return View(branchUpdateVM);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(branchUpdateVM);
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var service = GetBranchService<MerchantVM>();
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

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var service = GetBranchService<MerchantDetailVM>();
            var branch = await service.GetByIdAsync(id);

            return View(branch);
        }
        private IGenericService<TViewModel, Branch> GetBranchService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Branch>>();
        }

    }
}
