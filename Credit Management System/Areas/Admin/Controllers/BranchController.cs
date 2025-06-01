using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels.Branch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : BaseAdminController
    {
        private readonly IBranchService _branchService;
        private readonly IMerchantService _merchantService;

        public BranchController(IBranchService branchService, IMerchantService merchantService)
        {
            _branchService = branchService;
            _merchantService = merchantService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllWithMerchantsAsync();
            return View(branches);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var branchDetailVM = await _branchService.GetDetailByIdAsync(id);
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


                var data = await _branchService.AddAsync(branchCreateVM);

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

            var branchUpdateVM = await _branchService.GetUpdateByIdAsync(id);
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


                var data = await _branchService.UpdateAsync(branchUpdateVM);

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
