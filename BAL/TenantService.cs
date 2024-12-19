using DAL.Models.Entities;
using DAL.Models;
using DAL.Repositories;
using System;
using DAL.DTOs;
using System.Security.Cryptography;
using System.Text;
using BAL.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace BAL
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly BaseTenantRepository<ApplicationDbContext> _baseTenantRepository;
        private readonly IConfiguration _configuration;

        public TenantService(
            ITenantRepository tenantRepository,
             IConfiguration configuration,
            BaseTenantRepository<ApplicationDbContext> baseTenantRepository)
        {
            _tenantRepository = tenantRepository;
            _configuration = configuration;
            _baseTenantRepository = baseTenantRepository;
        }

        public void RegisterTenant(TenantDTO tenantDTO)
        {
            string tenantDbName = $"Tenant_{tenantDTO.TenantName.Replace(" ", "_")}";
            string connectionString = $"Server=DESKTOP-PAJ1QPJ\\SQLEXPRESS;Database={tenantDbName};TrustServerCertificate=True;Integrated Security=True;";

            DatabaseHelper.CreateDatabaseIfNotExists(tenantDbName);
            _baseTenantRepository.ApplyMigrations(connectionString);

            // Hash the admin password using SHA256
            var hashedPassword = HashPassword(tenantDTO.AdminPasswordHash);

            var tenant = new Tenant
            {
                TenantName = tenantDTO.TenantName,
                ConnectionString = connectionString,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                AdminEmail = tenantDTO.AdminEmail,
                AdminPasswordHash = hashedPassword, // Store the hashed password
                AdminPhoneNumber = tenantDTO.AdminPhoneNumber,
                AdminFullName = tenantDTO.AdminFullName
            };

            _tenantRepository.RegisterTenant(tenant);
        }

        public void UpdateTenant(int tenantId, UpdateTenantDTO tenantDTO)
        {
            var tenant = new Tenant
            {
                TenantId = tenantId,
                TenantName = tenantDTO.TenantName,
                IsActive = tenantDTO.IsActive,
                AdminEmail = tenantDTO.AdminEmail,
                AdminPasswordHash = tenantDTO.AdminPasswordHash, // Assume the password is hashed already
                AdminPhoneNumber = tenantDTO.AdminPhoneNumber,
                AdminFullName = tenantDTO.AdminFullName
            };

            _tenantRepository.UpdateTenant(tenant);
        }

        public void DeactivateTenant(int tenantId)
        {
            _tenantRepository.DeactivateTenant(tenantId);
        }

        public string Login(string adminEmail, string adminPassword)
        {
            var tenant = _tenantRepository.GetTenantByAdminEmail(adminEmail);

            if (tenant == null || tenant.AdminPasswordHash != HashPassword(adminPassword))
            {
                return null; // Invalid login credentials
            }

            // Generate JWT token with TenantId as part of the payload
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, adminEmail),
                new Claim(ClaimTypes.Role, "Admin"), // Add role or other claims as needed
                new Claim("TenantId", tenant.TenantId.ToString()) // Add TenantId claim
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiresInMinutes"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Method to hash the password (same as in your TenantService)
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public void DeleteTenant(int id)
        {
            if (id != null)
            {
                _tenantRepository.DeleteTenant(id);
            }
        }

        public bool CheckTenantExists(string tenantName)
        {
            return _tenantRepository.TenantExists(tenantName);
        }

    }
}
