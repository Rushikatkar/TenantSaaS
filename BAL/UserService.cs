// UserService.cs
using DAL.Repositories;
using DAL.Models.Entities;
using DAL.DTOs;
using System;
using Microsoft.Extensions.Configuration;

namespace BAL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void CreateUser(UserCreateDTO userCreateDTO, int tenantId)
        {
            // Hash the password before saving
            string hashedPassword = HashPassword(userCreateDTO.Password);

            var user = new User
            {
                UserName = userCreateDTO.UserName,
                Email = userCreateDTO.Email,
                PasswordHash = hashedPassword,
                Role = userCreateDTO.Role,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            _userRepository.CreateUser(tenantId, user);
        }

        public void UpdateUser(int userId, UserUpdateDTO userUpdateDTO, int tenantId)
        {
            var user = _userRepository.GetUserById(tenantId, userId);
            if (user != null)
            {
                user.UserName = userUpdateDTO.UserName ?? user.UserName;
                user.Email = userUpdateDTO.Email ?? user.Email;
                user.Role = userUpdateDTO.Role ?? user.Role;
                user.IsActive = userUpdateDTO.IsActive;

                _userRepository.UpdateUser(tenantId, user);
            }
        }

        public void DeleteUser(int userId, int tenantId)
        {
            _userRepository.DeleteUser(tenantId, userId);
        }
        public List<UserResponseDTO> GetUsers(int tenantId)
        {
            var users = _userRepository.GetUsers(tenantId);
            // Map to DTO to exclude sensitive data
            return users.Select(user => new UserResponseDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            }).ToList();
        }

        public UserResponseDTO GetUserById(int userId, int tenantId)
        {
            var user = _userRepository.GetUserById(tenantId, userId);
            if (user == null) return null;

            // Map to DTO to exclude sensitive data
            return new UserResponseDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
