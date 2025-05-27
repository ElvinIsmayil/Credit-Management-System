using Credit_Management_System.Areas.Admin.Controllers.Common;
using Credit_Management_System.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        public async Task<IActionResult> SeedRoles()
        {
            var roleNames = Enum.GetNames(typeof(Role));
            foreach (string roleName in roleNames)
            {
                if (!_roleManager.Roles.Any(r=>r.Name == roleName))
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
    }
}
