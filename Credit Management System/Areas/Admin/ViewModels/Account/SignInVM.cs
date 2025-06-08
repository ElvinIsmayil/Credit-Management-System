using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Areas.Admin.ViewModels.Account
{
    public class SignInVM
    {
        // Email validation with stricter rules
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Password with improved validation and error handling
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // "Remember Me" for session persistence
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}