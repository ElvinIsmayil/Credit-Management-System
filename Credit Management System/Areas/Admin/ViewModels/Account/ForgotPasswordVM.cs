using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Areas.Admin.ViewModels.Account
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
