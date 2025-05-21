using Credit_Management_System.Models;
using Credit_Management_System.Services.Interfaces;
using Credit_Management_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInVM signInVM)
        {
            if (!ModelState.IsValid) return View(signInVM);

            var user = await _userManager.FindByEmailAsync(signInVM.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(signInVM);
            }
            var result = await _signInManager.PasswordSignInAsync(signInVM.Email, signInVM.Password, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked out. Please try again later.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "You are not allowed to sign in.");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(signInVM);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpVM signUpVM)
        {
            if (!ModelState.IsValid) return View(signUpVM);
            var user = new AppUser
            {
                UserName = signUpVM.Username,
                Email = signUpVM.Email,
                PhoneNumber = signUpVM.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, signUpVM.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");
                return RedirectToAction("SignIn");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(signUpVM);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Lockout(string userId)
        {
            var existUser = await _userManager
                .Users
                .Where(u=>u.Id == userId)
                .FirstOrDefaultAsync();
            if (existUser is not null)
                existUser.LockoutEnabled = true;
            return View(existUser);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        //{
        //    if (!ModelState.IsValid) return View(forgotPasswordVM);
        //    var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
        //    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //    {
        //        return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    // Send email with the token
        //    return RedirectToAction("ForgotPasswordConfirmation", "Account");

        //}
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return View("EmailConfirmed");

            return View("Error");
        }


    }
}