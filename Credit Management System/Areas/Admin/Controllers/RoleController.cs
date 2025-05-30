using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Areas.Admin.ViewModels.Role;
using Credit_Management_System.Enums;
using Credit_Management_System.Models;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create()
        //{

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return View();
            }
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            var usersInRoleVM = users.Select(user => new UserInRoleVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FirstName + " " + user.LastName,
                JoinedDate = user.CreatedDate,
                ProfileImageUrl = user.ImageUrl
            }).ToList();

            var RoleDetailVM = new RoleDetailVM()
            {
                UserCount = users.Count,
                Users = usersInRoleVM,
                Id = role.Id,
                Name = role.Name,
            };

            return View(RoleDetailVM);
        }

        public async Task<IActionResult> SeedRoles()
        {
            var roleNames = Enum.GetNames(typeof(Role));
            foreach (string roleName in roleNames)
            {
                if (!_roleManager.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new()
                    {
                        Name = roleName
                    };
                    await _roleManager.CreateAsync(role);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
            var model = new AssignRoleVM();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                model.Users.Add(new UserVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CreatedDate = DateTime.Now,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    ImageUrl = user.ImageUrl,
                    LastLoginDate = DateTime.Now,
                });
            }

            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in roles)
            {
                model.Roles.Add(new RoleVM()
                {
                    Id = role.Id,
                    Name = role.Name
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(AssignRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                return View(model);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

    }
}
