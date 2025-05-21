using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.ViewModels
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 255 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [RegularExpression("^[0-9]{8}$", ErrorMessage = "Phone number must be exactly 8 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
