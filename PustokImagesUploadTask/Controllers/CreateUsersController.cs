using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokImagesUploadTask.Models;

namespace PustokImagesUploadTask.Controllers
{
    public class CreateUsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateUsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> CreateAdmin()
        {
            AppUser Admin = new AppUser()
            {
                FullName = "Super Admin",
                UserName = "SuperAdmin"
            };
            var result = await _userManager.CreateAsync(Admin, "SuperAdmin1");

            return Ok(result);
        }
        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("SuperAdmin");
            IdentityRole role2 = new IdentityRole("Admin");
            IdentityRole role3 = new IdentityRole("Member");

            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);
            return Ok("Created");
        }

        public async Task<IActionResult> AddRole()
        {
            AppUser user = await _userManager.FindByNameAsync("SuperAdmin");
            await _userManager.AddToRoleAsync(user, "SuperAdmin");
            return Ok("Role Added");
        }




        
   

}
}
