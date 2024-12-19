using DAL.DTOs;
using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface ITenantService
    {
        void RegisterTenant(TenantDTO tenantDTO);
        void UpdateTenant(int tenantId, UpdateTenantDTO tenantDTO);
        void DeactivateTenant(int tenantId);
        string Login(string adminEmail, string adminPassword);

        void DeleteTenant(int id);
        bool CheckTenantExists(string tenantName);

    }
}
