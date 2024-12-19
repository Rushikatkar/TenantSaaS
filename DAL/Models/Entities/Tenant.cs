using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Entities
{
    public class Tenant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TenantId { get; set; } // Primary Key

        [Required(ErrorMessage = "Tenant name is required.")]
        [StringLength(100, ErrorMessage = "Tenant name cannot exceed 100 characters.")]
        public string TenantName { get; set; } // Name of the tenant organization

        [Required(ErrorMessage = "Connection string is required.")]
        public string ConnectionString { get; set; } // Connection string for the tenant's database

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; } // Status of the tenant

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; } // Date the tenant was created

        [Required(ErrorMessage = "Admin email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string AdminEmail { get; set; }

        [Required(ErrorMessage = "Admin password hash is required.")]
        public string AdminPasswordHash { get; set; }

        // New properties
        [Required(ErrorMessage = "Admin phone number is required.")]
        [Range(1000000000, 9999999999, ErrorMessage = "Phone number must be a valid 10-digit number.")]
        public long AdminPhoneNumber { get; set; } // Admin's phone number

        [Required(ErrorMessage = "Admin full name is required.")]
        [StringLength(100, ErrorMessage = "Admin full name cannot exceed 100 characters.")]
        public string AdminFullName { get; set; } // Admin's full name
    }
}
