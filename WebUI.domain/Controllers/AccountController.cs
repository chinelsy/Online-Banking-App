using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using OnlineBanking.Domain.Entities;
using OnlineBanking.Domain.Enumerators;
using OnlineBanking.Domain.Interfaces.Services;
using OnlineBanking.Domain.Services;
using WebUI.domain.Interfaces.Services;
using WebUI.domain.Middlewares;
using WebUI.domain.Models;

namespace WebUI.domain.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICustomerService _customerService;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<User> signInManager,
            ICustomerService customerService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _customerService = customerService;
        }

        public async Task<IActionResult> LogIn(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");

            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(user?.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            var roles = await _userManager.GetRolesAsync(user);

            if (result.Succeeded)
            {
                // _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            }
            if (result.IsLockedOut)
            {
                // _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);

        }



        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }

            // Send an email with this link
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

            try
            {
                // await _emailSender.SendEmailAsync(model.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(model);

            }
            return View("ForgotPasswordConfirmation");
        }


        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public ViewResult AccessDenied()
        {
            return View();
        }


        public IActionResult EnrollCustomer()
        {
            return View(new EnrollCustomerViewModel());
        }

        public async Task<IActionResult> EnrollCustomer(EnrollCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var (result, user) = await AddUserAsync(new IdentityViewModel
            {
                FullName = $"{model.FirstName} {model.LastName}",
                Email = model.Email
            });


            if (result.Succeeded)
            {
               _customerService.Add(model, user, new ClaimsViewModel{Username = User.GetUsername()});

               TempData["EnrollSuccess"] = "Enrollment Was Successful!";

               //Send Mail To User With Credentials
               
               return RedirectToAction("Index", "Home");

            }
            ModelState.AddModelError(string.Empty, "An Error Occurred!!");
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }


        private async Task<(IdentityResult result, User user)> AddUserAsync(IdentityViewModel model)
        {

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Username
            };
            var randPassword = new Guid().ToString("N").Substring(0, 8);

            var result = await _userManager.CreateAsync(user, randPassword);
            return (result, user);
        }

    }
}
