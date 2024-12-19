using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string UserName { get; set; } // Required

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Required

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password must be between 8 and 50 characters.", MinimumLength = 8)]
        public string Password { get; set; } // Plain text password to hash before saving

        [Required(ErrorMessage = "Role is required.")]
        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be 'User' or 'Admin'.")]
        public string Role { get; set; } // Role can be "User" or "Admin"
    }

    public class UserUpdateDTO
    {
        [StringLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string UserName { get; set; } // Optional

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Optional

        [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be 'User' or 'Admin'.")]
        public string Role { get; set; } // Optional role update

        public bool IsActive { get; set; } // To activate or deactivate a user
    }

    public class UserResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
