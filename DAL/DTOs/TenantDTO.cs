using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs
{
    public class TenantDTO
    {
        [Required(ErrorMessage = "Tenant name is required.")]
        [StringLength(100, ErrorMessage = "Tenant name cannot exceed 100 characters.")]
        public string TenantName { get; set; }

        [Required(ErrorMessage = "Admin email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin password is required.")]
        [StringLength(50, ErrorMessage = "Password must be between 8 and 50 characters.", MinimumLength = 8)]
        public string AdminPasswordHash { get; set; }

        [Required(ErrorMessage = "Admin phone number is required.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number must be a valid 10-digit number.")]
        public long AdminPhoneNumber { get; set; }

        [Required(ErrorMessage = "Admin full name is required.")]
        [StringLength(100, ErrorMessage = "Admin full name cannot exceed 100 characters.")]
        public string AdminFullName { get; set; }
    }

    public class UpdateTenantDTO
    {
        [Required(ErrorMessage = "Tenant name is required.")]
        [StringLength(100, ErrorMessage = "Tenant name cannot exceed 100 characters.")]
        public string TenantName { get; set; }

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Admin email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin password is required.")]
        [StringLength(50, ErrorMessage = "Password must be between 8 and 50 characters.", MinimumLength = 8)]
        public string AdminPasswordHash { get; set; }

        [Required(ErrorMessage = "Admin phone number is required.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number must be a valid 10-digit number.")]
        public long AdminPhoneNumber { get; set; }

        [Required(ErrorMessage = "Admin full name is required.")]
        [StringLength(100, ErrorMessage = "Admin full name cannot exceed 100 characters.")]
        public string AdminFullName { get; set; }
    }

    public class LoginDTO
    {
        [Required(ErrorMessage = "Admin email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin password is required.")]
        [StringLength(50, ErrorMessage = "Password must be between 8 and 50 characters.", MinimumLength = 8)]
        public string AdminPassword { get; set; }
    }
}
