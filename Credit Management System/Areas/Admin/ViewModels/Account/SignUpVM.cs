﻿using System.ComponentModel.DataAnnotations;

public class SignUpVM
{
    // First Name
    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First Name can only contain letters.")]
    public string FirstName { get; set; }

    // Last Name
    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Last Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last Name can only contain letters.")]
    public string LastName { get; set; }

    // Email
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }

    // Username
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 30 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    public string Username { get; set; }

    // Password
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }

    // Confirm Password
    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }

    // Phone Number
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    public string PhoneNumber { get; set; }

    // Terms and Conditions Agreement
    [Required(ErrorMessage = "You must accept the terms and conditions.")]
    public bool Toc { get; set; }
}