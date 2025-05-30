using Credit_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Areas.Admin.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly UserManager<AppUser> _userManager;

    public HeaderViewComponent(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var email = User.Identity?.Name;
        if (string.IsNullOrEmpty(email))
            return View(null);

        var existUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        var userVM = new UserVM()
        {
            CreatedDate = existUser.CreatedDate,
            Email = existUser.Email,
            EmailConfirmed = existUser.EmailConfirmed,
            FirstName = existUser.FirstName,
            LastName = existUser.LastName,
            Id = existUser.Id,
            ImageUrl = existUser.ImageUrl,
            LastLoginDate = existUser.LastLoginDate,
            Role = (await _userManager.GetRolesAsync(existUser)).ToList()
        };
        return View(userVM);
    }

}