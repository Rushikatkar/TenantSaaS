// UserRepository.cs
using Microsoft.EntityFrameworkCore;
using DAL.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Func<string, ApplicationDbContext> _dbContextFactory;
        private readonly ApplicationDbContext _masterDbContext; // For fetching tenant connection strings
        public UserRepository(Func<string, ApplicationDbContext> dbContextFactory, ApplicationDbContext masterDbContext)
        {
            _dbContextFactory = dbContextFactory;
            _masterDbContext = masterDbContext;
        }

        private ApplicationDbContext GetDbContext(int tenantId)
        {
            // Get tenant connection string from the database
            var connectionString = GetTenantConnectionString(tenantId);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Connection string for tenant {tenantId} not found.");
            }

            return _dbContextFactory(connectionString);
        }

        private string GetTenantConnectionString(int tenantId)
        {
            // Fetch tenant connection string using the injected master database context
            var tenant = _masterDbContext.Tenants.FirstOrDefault(t => t.TenantId == tenantId);
            return tenant?.ConnectionString;
        }

        public User GetUserById(int tenantId, int userId)
        {
            var dbContext = GetDbContext(tenantId);
            return dbContext.Users.Find(userId);
        }


        public List<User> GetUsers(int tenantId)
        {
            var dbContext = GetDbContext(tenantId);
            return dbContext.Users.ToList();
        }

        public void CreateUser(int tenantId, User user)
        {
            var dbContext = GetDbContext(tenantId);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public void UpdateUser(int tenantId, User user)
        {
            var dbContext = GetDbContext(tenantId);
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
        }

        public void DeleteUser(int tenantId, int userId)
        {
            var dbContext = GetDbContext(tenantId);
            var user = dbContext.Users.Find(userId);
            if (user != null)
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}
