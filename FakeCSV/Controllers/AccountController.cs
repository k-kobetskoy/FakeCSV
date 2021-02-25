using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCSV.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FakeCSV.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<AccountController> logger;
        public AccountController(IConfiguration configuration,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 ILogger<AccountController> logger)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = userManager.Users.FirstOrDefault(u => u.Email == model.Email);
            logger.LogInformation("trying to login user {0}", model.Email);

            if (user is null)
            {
                logger.LogError("there is no such user {0}", model.Email);
                ModelState.AddModelError("", "there is no such user or wrong password!");
                return View(model);
            }
            
            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                logger.LogError("error while trying to login {0}", model.Email);
                ModelState.AddModelError("", "there is no such user or wrong password!");
                return View(model);
            }

            logger.LogInformation(" user {0} successfuly signed in", model.Email);
            logger.LogInformation(" redirecting to returnUrl");
            if (Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            logger.LogError(" redirect failed! returnUrl is not local url");
            logger.LogInformation(" redirecting to home page");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View("Register", model);

            var role = configuration.GetSection("Roles")["Users"].ToUpper();
            if (!AnyUserExist())
                role = configuration.GetSection("Roles")["Administrators"].ToUpper();

            var userName = model.Email.Remove(model.Email.IndexOf('@'));

            logger.LogInformation("creating new user entry {0}", userName);
            IdentityUser user = new IdentityUser() { UserName = userName, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    logger.LogError("error while creating user {0}: {1}", userName, error.Description);
                }
                return View(model);
            }
            logger.LogInformation("user entry {0} created successfuly", userName);


            logger.LogInformation("assigning user {0} to role {1}", userName, role);
            result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    logger.LogError("error while assigning user {0} to role {1}: {2}", userName, role, error.Description);
                }
                return View(model);
            }
            logger.LogInformation("user {0} assigned to role {1} successfuly", userName, role);

            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string Email)
        {
            if (AnyUserExist())
                return Json(FindSame(Email));

            return Json(true);
        }

        private bool AnyUserExist() => userManager.Users.Any();
        private bool FindSame(string email) => !userManager.Users.Select(u => u.Email == email).Any();
    }
}
