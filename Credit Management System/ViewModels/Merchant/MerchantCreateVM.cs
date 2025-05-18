using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Credit_Management_System.ViewModels.Merchant
{
    public class MerchantCreateVM
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 150 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-']+$", ErrorMessage = "Name contains invalid characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Address must be between 1 and 250 characters.")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Phone number must be between 1 and 20 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[0-9\s\-()]+$", ErrorMessage = "Phone number contains invalid characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Email must be between 1 and 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Merchant Image")]
        public IFormFile? Image { get; set; }
    }
}
