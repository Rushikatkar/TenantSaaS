using BAL;
using DAL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly ILogger<TenantController> _logger;

        public TenantController(ITenantService tenantService, ILogger<TenantController> logger)
        {
            _tenantService = tenantService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult RegisterTenant([FromBody] TenantDTO tenantDTO)
        {
            if (tenantDTO == null || string.IsNullOrEmpty(tenantDTO.TenantName))
            {
                _logger.LogWarning("Attempted to register a tenant without a valid name.");
                return BadRequest(new { message = "Tenant name is required!" });
            }

            // Check if a tenant with the same name already exists
            bool isTenantExists = _tenantService.CheckTenantExists(tenantDTO.TenantName);
            if (isTenantExists)
            {
                _logger.LogWarning("Tenant registration failed. A tenant with the same name already exists: {TenantName}", tenantDTO.TenantName);
                return Conflict(new { message = "A tenant with the same name already exists!" });
            }

            _tenantService.RegisterTenant(tenantDTO);
            _logger.LogInformation("Tenant registered successfully: {TenantName}", tenantDTO.TenantName);
            return Ok(new { message = "Tenant registered successfully!" });
        }

        [HttpPut("update/{tenantId}")]
        public IActionResult UpdateTenant(int tenantId, [FromBody] UpdateTenantDTO tenantDTO)
        {
            if (tenantDTO == null || string.IsNullOrEmpty(tenantDTO.TenantName))
            {
                _logger.LogWarning("Attempted to update tenant with invalid data. TenantId: {TenantId}", tenantId);
                return BadRequest(new { message = "Tenant name is required!" });
            }

            _tenantService.UpdateTenant(tenantId, tenantDTO);
            _logger.LogInformation("Tenant updated successfully: {TenantId}", tenantId);
            return Ok(new { message = "Tenant updated successfully!" });
        }

        [HttpPut("deactivate/{tenantId}")]
        public IActionResult DeactivateTenant(int tenantId)
        {
            _tenantService.DeactivateTenant(tenantId);
            _logger.LogInformation("Tenant deactivated successfully: {TenantId}", tenantId);
            return Ok(new { message = "Tenant deactivated successfully!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var token = _tenantService.Login(loginDto.AdminEmail, loginDto.AdminPassword);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Login attempt failed for email: {AdminEmail}", loginDto.AdminEmail);
                return Unauthorized("Invalid credentials");
            }

            _logger.LogInformation("Login successful for email: {AdminEmail}", loginDto.AdminEmail);
            return Ok(new { token });
        }

        [HttpDelete("delete/id")]
        public IActionResult Delete(int id)
        {
            if (id != null)
            {
                _tenantService.DeleteTenant(id);
                _logger.LogInformation("Tenant deleted successfully: {TenantId}", id);
                return Ok("Tenant is Deleted Successfully");
            }

            _logger.LogWarning("Invalid tenant deletion attempt. TenantId: {TenantId} is null", id);
            return StatusCode(500, "Please enter valid id");
        }
    }
}
