using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Areas.Admin.ViewModels.User
{
    public class UserVM
    {
        public string Id { get; set; } = null!;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        public bool EmailConfirmed { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Joined Date")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Login Date")]
        [DataType(DataType.DateTime)]
        public DateTime? LastLoginDate { get; set; }

        public string? Role { get; set; }
    }
}
