namespace Credit_Management_System.Areas.Admin.ViewModels.User
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDetailVM
    {
        [Required]
        public string Id { get; set; } = null!;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Joined Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Login Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", NullDisplayText = "Never logged in")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Role")]
        public string? Role { get; set; }

        // Add any extra detailed fields here, for example:
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }
    }

}
