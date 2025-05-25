using Credit_Management_System.Areas.Admin.ViewModels.Account;
using Credit_Management_System.Helpers;
using Credit_Management_System.Infrastructure.Interfaces;
using Credit_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace Credit_Management_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public AccountController(RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
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
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please fix the form validation errors.";
                TempData["AlertType"] = "swal";
                return View(signInVM);
            }

            var user = await _userManager.FindByEmailAsync(signInVM.Email);
            if (user == null)
            {
                TempData[AlertHelper.Error] = "Invalid email or password.";
                TempData["AlertType"] = "swal";
                return View(signInVM);
            }

            var result = await _signInManager.PasswordSignInAsync(signInVM.Email, signInVM.Password, true, false);
            if (result.Succeeded)
            {
                TempData[AlertHelper.Success] = "Welcome back!";
                TempData["AlertType"] = "toastr";
                return RedirectToAction("Index", "Dashboard");
            }

            if (result.IsLockedOut)
            {
                TempData[AlertHelper.Error] = "Your account is locked out. Please try again later.";
            }
            else if (result.IsNotAllowed)
            {
                TempData[AlertHelper.Error] = "You are not allowed to sign in.";
            }
            else
            {
                TempData[AlertHelper.Error] = "Invalid login attempt.";
            }
            TempData["AlertType"] = "swal";
            return View(signInVM);
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
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please fill out the form correctly.";
                TempData["AlertType"] = "toastr";
                return View(signUpVM);
            }

            if (!signUpVM.Toc)
            {
                TempData[AlertHelper.Error] = "You must agree to the terms and conditions.";
                TempData["AlertType"] = "toastr";
                return View(signUpVM);
            }

            var existingUser = await _userManager.FindByEmailAsync(signUpVM.Email);
            if (existingUser != null)
            {
                TempData[AlertHelper.Error] = "This email is already registered.";
                TempData["AlertType"] = "toastr";
                return View(signUpVM);
            }

            var user = new AppUser
            {
                UserName = signUpVM.Email,
                Email = signUpVM.Email,
                FirstName = signUpVM.FirstName,
                LastName = signUpVM.LastName
            };

            var result = await _userManager.CreateAsync(user, signUpVM.Password);

            if (!result.Succeeded)
            {
                TempData[AlertHelper.Error] = string.Join(" ", result.Errors.Select(e => e.Description));
                TempData["AlertType"] = "toastr";
                return View(signUpVM);
            }

            // Generate email confirmation token and URL-encode it
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = encodedToken }, protocol: HttpContext.Request.Scheme);

            var emailBody = EmailTemplateHelper.GetEmailConfirmationHtml(confirmationLink);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);

            TempData[AlertHelper.Success] = "Registration successful! Please check your email to confirm your account.";
            TempData["AlertType"] = "toastr";
            TempData["Email"] = user.Email;

            return RedirectToAction("EmailVerification");
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || await _userManager.IsEmailConfirmedAsync(user))
            {
                TempData[AlertHelper.Error] = "Invalid or already confirmed email.";
                TempData["AlertType"] = "toastr";
                return RedirectToAction("SignIn");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = encodedToken }, protocol: HttpContext.Request.Scheme);

            var emailBody = $"Click <a href='{confirmationLink}'>here</a> to confirm your email.";
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailBody);

            TempData[AlertHelper.Success] = "Confirmation email resent.";
            TempData["AlertType"] = "toastr";
            return RedirectToAction("EmailVerification");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction("SignIn", "Account");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("SignIn", "Account");

            // URL-decode token before using it
            token = HttpUtility.UrlDecode(token);

            if (await _userManager.IsEmailConfirmedAsync(user))
                return RedirectToAction("SignIn", "Account");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("ConfirmEmailConfirmation");
            }

            ViewBag.Errors = result.Errors;
            return View("Error");
        }

        [HttpGet]
        public IActionResult ConfirmEmailConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailVerification()
        {
            ViewBag.Email = TempData["Email"] as string;
            return View();
        }

        #region Password

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData[AlertHelper.Error] = "Please enter a valid email.";
                TempData["AlertType"] = "toastr";
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                TempData["Email"] = model.Email;
                TempData["AlertType"] = "toastr";
                // Don't reveal user existence
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new
            {
                email = user.Email,
                token = token
            }, Request.Scheme);

            var emailBody = EmailTemplateHelper.GetPasswordResetHtml(resetLink);
            await _emailSender.SendEmailAsync(user.Email, "Password Reset Request", emailBody);

            TempData[AlertHelper.Success] = "Password reset email sent.";
            TempData["AlertType"] = "toastr";
            TempData["Email"] = model.Email;
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            ViewBag.Email = TempData["Email"] as string;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendPasswordConfirmation(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Please provide your email.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Don't reveal that the user does not exist for security reasons
                return RedirectToAction("ResendPasswordConfirmationConfirmation");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, "Email is already confirmed.");
                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            var confirmationLink = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = encodedToken }, protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");

            return RedirectToAction("ResendPasswordConfirmationConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
                return RedirectToAction("Index", "Home");

            var model = new ResetPasswordVM { Email = email, Token = token };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        #endregion

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
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (existUser is not null)
                existUser.LockoutEnabled = true;

            return View(existUser);
        }
    }
}
