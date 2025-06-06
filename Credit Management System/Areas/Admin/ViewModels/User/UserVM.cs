﻿using System.ComponentModel.DataAnnotations;

public class UserVM
{
    [Required]
    public string Id { get; set; } = null!;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = null!;
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";

    [EmailAddress]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }


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

    public List<string>? Role { get; set; }
}
