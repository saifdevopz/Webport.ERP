using Microsoft.EntityFrameworkCore;
using Webport.ERP.Common.Infrastructure.Interceptors;
using Webport.ERP.Common.Infrastructure.Outbox;
using Webport.ERP.Identity.Infrastructure.Common;

namespace Webport.ERP.Identity.Infrastructure.Database;

public sealed class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
{
    public DbSet<TenantM> Tenants => Set<TenantM>();
    public DbSet<UserM> Users => Set<UserM>();
    public DbSet<RoleM> Roles => Set<RoleM>();
    public DbSet<PermissionM> Permissions => Set<PermissionM>();
    public DbSet<RolePermissionM> RolePermissions => Set<RolePermissionM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Schema
        modelBuilder.HasDefaultSchema(IdentityConstants.Schema);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        // Interceptors
        optionsBuilder.AddInterceptors(new AuditableEntityInterceptor());
        optionsBuilder.AddInterceptors(new InsertOutboxMessagesInterceptor());
    }
}