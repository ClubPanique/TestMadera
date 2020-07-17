using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TestMadera.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public SecurityController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await SignIn(user, password);
                
                if (result.Succeeded)
                {
                    return Redirect("Home");
                }
            }
            return Redirect("Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = "UserName",
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var signInResult = await SignIn(user, password);
                
                if (signInResult.Succeeded)
                {
                    return Redirect("Home");
                }
                else
                {
                    ViewBag.Errors = signInResult.IsNotAllowed;
                    return Redirect("Home");
                }
            }
            else
            {
                ViewBag.Errors = result.Errors;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("Home");
        }

        private async Task<SignInResult> SignIn(IdentityUser user, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

            return result;
        }

        
    }
}
