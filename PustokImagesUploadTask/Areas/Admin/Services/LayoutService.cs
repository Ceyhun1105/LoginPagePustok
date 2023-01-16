using Microsoft.AspNetCore.Identity;
using PustokImagesUploadTask.Models;

namespace PustokImagesUploadTask.Areas.Admin.Services
{
    public class LayoutService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<AppUser> GetUser()
        {
            string name = _httpContextAccessor.HttpContext.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(name);
            return user;
        }

    }
}
