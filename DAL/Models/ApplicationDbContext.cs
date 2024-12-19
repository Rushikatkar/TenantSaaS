using DAL.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Shared tables for the master database
    public DbSet<Tenant> Tenants { get; set; }

    // Tenant-specific tables (will only appear in tenant databases)
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // We will exclude the Users table for the master database only after migration.
        // This logic should be handled in the runtime environment, not during design-time.
    }
}
