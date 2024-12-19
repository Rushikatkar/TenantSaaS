using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BaseTenantRepository<TContext> where TContext : DbContext
    {
        private readonly Func<string, TContext> _contextFactory;

        public BaseTenantRepository(Func<string, TContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ApplyMigrations(string connectionString)
        {
            using var context = _contextFactory(connectionString);
            context.Database.Migrate(); // Ensures migrations are applied
        }

        public TContext GetTenantContext(string connectionString)
        {
            return _contextFactory(connectionString);
        }
    }
}
