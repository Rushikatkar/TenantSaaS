using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.Models
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Use your actual connection string here for the Master Database
            var connectionString = "Server=DESKTOP-PAJ1QPJ\\SQLEXPRESS;Database=MasterTenantDb;TrustServerCertificate=True;Integrated Security=True;";

            optionsBuilder.UseSqlServer(connectionString);

            // Return the ApplicationDbContext with the correct options
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
