using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ComponentModel.DataAnnotations;

namespace PustokImagesUploadTask.ViewModels
{
    public class MemberRegisterViewModel
    {
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }

        [StringLength(maximumLength: 20)]
        public string UserName { get; set; }
        [StringLength(maximumLength: 70), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 8), DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
