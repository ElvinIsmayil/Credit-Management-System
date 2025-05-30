namespace Credit_Management_System.Areas.Admin.ViewModels.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserUpdateVM
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }
    }

}
