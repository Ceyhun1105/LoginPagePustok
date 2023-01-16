using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokImagesUploadTask.Areas.Admin.ViewModels;
using PustokImagesUploadTask.Models;

namespace PustokImagesUploadTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminVm)
        {
            if (!ModelState.IsValid) return View(adminVm);
            AppUser admin = await _userManager.FindByNameAsync(adminVm.Username);
            if(admin is null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(adminVm);
            }

            var result = await _signInManager.PasswordSignInAsync(admin, adminVm.Password, false, false);
            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(adminVm);
            }
            return RedirectToAction("index","dashboard");
        }
        public async Task<IActionResult> LogOut ()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
