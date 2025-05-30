using System.ComponentModel.DataAnnotations;
namespace Credit_Management_System.Areas.Admin.ViewModels.User
{

    public class UserCreateVM
    {
        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name can't exceed 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name can't exceed 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "User Role")]
        [Required(ErrorMessage = "User role is required.")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }


    }

}
