using Credit_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Credit_Management_System.Areas.Admin.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly UserManager<AppUser>  _userManager;

    public HeaderViewComponent(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var existUser = _userManager.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
        return View(existUser);
    }
}