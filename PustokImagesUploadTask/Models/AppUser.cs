using Microsoft.AspNetCore.Identity;

namespace PustokImagesUploadTask.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
