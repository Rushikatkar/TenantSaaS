using DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IUserRepository
    {
        User GetUserById(int tenantId, int userId);

        List<User> GetUsers(int tenantId);

        void CreateUser(int tenantId, User user);

        void UpdateUser(int tenantId, User user);

        void DeleteUser(int tenantId, int userId);

    }
}
