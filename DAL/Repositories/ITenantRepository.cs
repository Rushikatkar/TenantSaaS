using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface ITenantRepository
    {
        void RegisterTenant(Tenant tenant);
        void UpdateTenant(Tenant tenant);
        void DeactivateTenant(int tenantId);
        Tenant GetTenantByAdminEmail(string adminEmail);

        void DeleteTenant(int id);
        bool TenantExists(string tenantName);

    }
}
