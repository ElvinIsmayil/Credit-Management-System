using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Areas.Admin.ViewModels.User;
using Credit_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateVM userCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userCreateVM);
            }
            var user = await _userService.AddAsync(userCreateVM);
            if (user == null)
            {
                ModelState.AddModelError("", "User creation failed.");
                return View(userCreateVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();

            var userDetailVM = await _userService.GetDetailByIdAsync(id);
            if (userDetailVM == null)
            {
                return NotFound();
            }

            return View(userDetailVM);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();
            var userUpdateVM = await _userService.GetUpdateByIdAsync(id);
            if (userUpdateVM == null)
            {
                return NotFound();
            }
            return View(userUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateVM userUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userUpdateVM);
            }
            var user = await _userService.UpdateAsync(userUpdateVM);
            if (user == null)
            {
                ModelState.AddModelError("", "User update failed.");
                return View(userUpdateVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();
            var result = await _userService.DeleteAsync(id);
            if (!result)
            {
                ModelState.AddModelError("", "User deletion failed.");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
