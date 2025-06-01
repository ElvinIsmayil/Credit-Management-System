using Credit_Management_System.Enums;

namespace Credit_Management_System.Areas.Admin.ViewModels.Profile
{
    public class BaseProfileVM
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName => $"{FirstName} {LastName}";
        public DateOnly BirthDate { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool IsActive { get; set; }
    }
}
