using Credit_Management_System.Models;
using Credit_Management_System.Services.Implementations;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IMerchantService _merchantService;
        private readonly IServiceProvider _serviceProvider;

        public BranchController(
            IBranchService branchService,
            IMerchantService merchantService,
            IServiceProvider serviceProvider)
        {
            _branchService = branchService;
            _merchantService = merchantService;
            _serviceProvider = serviceProvider;
        }

        private IGenericService<TViewModel, Branch> GetBranchService<TViewModel>() where TViewModel : class
        {
            return _serviceProvider.GetRequiredService<IGenericService<TViewModel, Branch>>();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllBranchesAsync(); // returns List<BranchVM>
            return View(branches);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var branchDetailVM = await _branchService.GetBranchDetailByIdAsync(id); // returns BranchDetailVM
            if (branchDetailVM == null)
                return NotFound();

            return View(branchDetailVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var merchants = await _merchantService.GetAllAsync();
            var branchCreateVM = new BranchCreateVM
            {
                Merchants = merchants.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList()
            };
            return View(branchCreateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchCreateVM branchCreateVM)
        {
            try
            {
                var merchants = await _merchantService.GetAllAsync();
                branchCreateVM.Merchants = merchants.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();

                if (!ModelState.IsValid)
                    return View(branchCreateVM);

                var service = GetBranchService<BranchCreateVM>();
                var data = await service.AddAsync(branchCreateVM);

                if (data == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create branch.");
                    return View(branchCreateVM);
                }

                return RedirectToAction("Index");
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
            var branchUpdateVM = await service.GetByIdAsync(id);
            if (branchUpdateVM == null)
                return NotFound();

            var merchants = await _merchantService.GetAllAsync();
            branchUpdateVM.Merchants = merchants.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();

            return View(branchUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BranchUpdateVM branchUpdateVM)
        {
            try
            {
                var merchants = await _merchantService.GetAllAsync();
                branchUpdateVM.Merchants = merchants.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _branchService.DeleteAsync(id);

                if (!result)
                    return Json(new { success = false, message = "Delete failed. Item not found or already deleted." });

                return Json(new { success = true, message = "Item deleted successfully." });
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting the item." });
            }
        }
    }
}
