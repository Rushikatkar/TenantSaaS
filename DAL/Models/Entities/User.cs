using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; } // Primary Key

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(100, ErrorMessage = "User name cannot exceed 100 characters.")]
        public string UserName { get; set; } // User's name

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // User's email

        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; } // Encrypted password

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; } // Role of the user (e.g., Admin, User)

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; } // Active or inactive user status

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; } // Date the user was created
    }
}
