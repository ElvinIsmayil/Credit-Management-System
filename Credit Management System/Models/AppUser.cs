using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Credit_Management_System.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
