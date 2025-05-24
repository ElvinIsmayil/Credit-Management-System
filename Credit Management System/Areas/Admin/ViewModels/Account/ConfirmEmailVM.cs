using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Areas.Admin.ViewModels.Account
{
    public class ConfirmEmailVM
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
