using DAL.Models;
using DAL.Models.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void RegisterTenant(Tenant tenant)
        {
            _context.Tenants.Add(tenant);
            _context.SaveChanges();
        }

        public void UpdateTenant(Tenant tenant)
        {
            var existingTenant = _context.Tenants.FirstOrDefault(t => t.TenantId == tenant.TenantId);
            if (existingTenant != null)
            {
                existingTenant.TenantName = tenant.TenantName;
                existingTenant.IsActive = tenant.IsActive;
                existingTenant.AdminEmail = tenant.AdminEmail;
                existingTenant.AdminPasswordHash = tenant.AdminPasswordHash;
                existingTenant.AdminPhoneNumber = tenant.AdminPhoneNumber;
                existingTenant.AdminFullName = tenant.AdminFullName;

                _context.SaveChanges();
            }
        }

        public void DeactivateTenant(int tenantId)
        {
            var tenant = _context.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
            if (tenant != null)
            {
                tenant.IsActive = false;
                _context.SaveChanges();
            }
        }

        public Tenant GetTenantByAdminEmail(string adminEmail)
        {
            // Retrieve tenant by admin email
            return _context.Tenants.SingleOrDefault(t => t.AdminEmail == adminEmail);
        }

        public void DeleteTenant(int id)
        {
           var tenant= _context.Tenants.Find(id);

            if (tenant == null)
            {
                throw new ArgumentException($"Tenant with ID {id} does not exist.");
            }

            _context.Tenants.Remove(tenant);
            _context.SaveChanges();
        }

        public bool TenantExists(string tenantName)
        {
            return _context.Tenants.Any(t => t.TenantName == tenantName);
        }

    }
}
