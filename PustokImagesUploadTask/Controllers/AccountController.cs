using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokImagesUploadTask.Areas.Admin.ViewModels;
using PustokImagesUploadTask.Models;
using PustokImagesUploadTask.ViewModels;

namespace PustokImagesUploadTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signinManager;

        public AccountController(UserManager<AppUser> userManager, AppDbContext context,RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signinManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
             _signinManager = signinManager;
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterView)
        {
            if (!ModelState.IsValid) return View(memberRegisterView);
            AppUser user = null;

            user = await _userManager.FindByNameAsync(memberRegisterView.UserName);
            if (user != null)
            {
                ModelState.AddModelError("UserName", "Already Exist!");
                return View(memberRegisterView);
            }
            user = _context.Users.FirstOrDefault(x=>x.NormalizedEmail==memberRegisterView.Email.ToUpper());
            if (user != null)
            {
                ModelState.AddModelError("Email", "Already Exist!");
                return View(memberRegisterView);
            }

            user = new AppUser()
            {
                FullName = memberRegisterView.FullName,
                Email = memberRegisterView.Email,
                UserName = memberRegisterView.UserName,
            };
            var result = await _userManager.CreateAsync(user,memberRegisterView.Password);
            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            var result1 = await _userManager.AddToRoleAsync(user, "Member");
            if (!result1.Succeeded)
            {
                return View(memberRegisterView);
            }

            await _signinManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction ("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginViewModel)
        {
            
                if (!ModelState.IsValid) return View(memberLoginViewModel);
                AppUser member = await _userManager.FindByNameAsync(memberLoginViewModel.UserName);
                if (member is null)
                {
                    ModelState.AddModelError("", "Username or Password is incorrect");
                    return View(memberLoginViewModel);
                }

                var result = await _signinManager.PasswordSignInAsync(member, memberLoginViewModel.Password, false, false);
                if (!result.Succeeded)
                {

                    ModelState.AddModelError("", "Username or Password is incorrect");
                    return View(member);
                }
                return RedirectToAction("index", "home");
            }

        public async Task<IActionResult> LogOut()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }




    }
}
