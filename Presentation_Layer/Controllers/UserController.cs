using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BAL;
using DAL.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // Helper to extract TenantId from the JWT token
        private int GetTenantIdFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var tenantIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "TenantId");
            return tenantIdClaim != null ? Convert.ToInt32(tenantIdClaim.Value) : 0;
        }

        // Create User
        [HttpPost("register")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            int tenantId = GetTenantIdFromToken();
            if (tenantId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt for user creation. TenantId missing in token.");
                return Unauthorized();
            }

            _userService.CreateUser(userCreateDTO, tenantId);
            _logger.LogInformation("User created successfully. TenantId: {TenantId}, UserName: {UserName}", tenantId, userCreateDTO.UserName);
            return Ok("User created successfully.");
        }

        // Update User
        [HttpPut("update/{userId}")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            int tenantId = GetTenantIdFromToken();
            if (tenantId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt for user update. TenantId missing in token.");
                return Unauthorized();
            }

            _userService.UpdateUser(userId, userUpdateDTO, tenantId);
            _logger.LogInformation("User updated successfully. TenantId: {TenantId}, UserId: {UserId}", tenantId, userId);
            return Ok("User updated successfully.");
        }

        // Delete User
        [HttpDelete("delete/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int userId)
        {
            int tenantId = GetTenantIdFromToken();
            if (tenantId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt for user deletion. TenantId missing in token.");
                return Unauthorized();
            }

            _userService.DeleteUser(userId, tenantId);
            _logger.LogInformation("User deleted successfully. TenantId: {TenantId}, UserId: {UserId}", tenantId, userId);
            return Ok("User deleted successfully.");
        }

        // Get all Users
        [HttpGet("getall")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetUsers()
        {
            int tenantId = GetTenantIdFromToken();
            if (tenantId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt to get all users. TenantId missing in token.");
                return Unauthorized();
            }

            var users = _userService.GetUsers(tenantId);
            _logger.LogInformation("Fetched all users for TenantId: {TenantId}", tenantId);
            return Ok(users);
        }

        // Get User by Id
        [HttpGet("byid/{userId}")]
        [Authorize(Roles = "Admin, User")]
        public IActionResult GetUserById(int userId)
        {
            int tenantId = GetTenantIdFromToken();
            if (tenantId == 0)
            {
                _logger.LogWarning("Unauthorized access attempt to fetch user by id. TenantId missing in token.");
                return Unauthorized();
            }

            var user = _userService.GetUserById(userId, tenantId);
            if (user == null)
            {
                _logger.LogWarning("User not found. TenantId: {TenantId}, UserId: {UserId}", tenantId, userId);
                return NotFound(new { message = "User not found." });
            }

            _logger.LogInformation("Fetched user by id successfully. TenantId: {TenantId}, UserId: {UserId}", tenantId, userId);
            return Ok(user);
        }
    }
}
