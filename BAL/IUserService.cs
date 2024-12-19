using DAL.DTOs;
using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public interface IUserService
    {
        // Create a new user
        void CreateUser(UserCreateDTO userCreateDTO, int tenantId);

        // Update an existing user
        void UpdateUser(int userId, UserUpdateDTO userUpdateDTO, int tenantId);

        // Delete a user
        void DeleteUser(int userId, int tenantId);

        List<UserResponseDTO> GetUsers(int tenantId);

        // Get a user by ID for a specific tenant
        UserResponseDTO GetUserById(int userId, int tenantId);
    }
}
