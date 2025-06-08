using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Areas.Admin.ViewModels.Role;
using Credit_Management_System.Models;
using Credit_Management_System.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles?.ToList();
            var roleVMs = roles.Select(role => new RoleVM
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
            return View(roleVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var usersInRoleVM = usersInRole.Select(user => new UserInRoleVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FirstName + " " + user.LastName,
                JoinedDate = user.CreatedDate,
                ProfileImageUrl = user.ImageUrl
            }).OrderBy(u => u.FullName).ToList();

            var allUsers = await _userManager.Users.ToListAsync();
            var availableUsers = allUsers
                .Where(u => !usersInRole.Any(ur => ur.Id == u.Id))
                .Select(user => new UserInRoleVM
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FirstName + " " + user.LastName,
                    JoinedDate = user.CreatedDate,
                    ProfileImageUrl = user.ImageUrl
                })
                .OrderBy(u => u.FullName)
                .ToList();

            var model = new RoleDetailVM()
            {
                Id = role.Id,
                Name = role.Name,
                UserCount = usersInRole.Count,
                Users = usersInRoleVM, 
                AvailableUsers = availableUsers 
            };

            return View(model);
        }

        public async Task<IActionResult> SeedRoles()
        {
            string[] roleNames = { Roles.Admin, Roles.Employee, Roles.Customer, Roles.User };

            foreach (string roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    IdentityRole role = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(role);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string SelectedUserId, string RoleName, string RoleId)
        {
            if (string.IsNullOrEmpty(SelectedUserId))
            {
                TempData["ErrorMessage"] = "Please select a user to assign.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                TempData["ErrorMessage"] = $"User '{user.FirstName} {user.LastName}' is already in the '{role.Name}' role.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{user.FirstName} {user.LastName}' successfully assigned to '{role.Name}' role.";
                return RedirectToAction("Detail", new { id = RoleId }); 
            }
            else
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Failed to assign user to role: {errors}";
                return RedirectToAction("Detail", new { id = RoleId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(string UserId, string RoleName, string RoleId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            if (!await _userManager.IsInRoleAsync(user, RoleName))
            {
                TempData["ErrorMessage"] = $"User '{user.FirstName} {user.LastName}' is not in the '{RoleName}' role.";
                return RedirectToAction("Detail", new { id = RoleId });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"User '{user.FirstName} {user.LastName}' successfully removed from '{RoleName}' role.";
            }
            else
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Failed to remove user from role: {errors}";
            }

            return RedirectToAction("Detail", new { id = RoleId });
        }
    }
}