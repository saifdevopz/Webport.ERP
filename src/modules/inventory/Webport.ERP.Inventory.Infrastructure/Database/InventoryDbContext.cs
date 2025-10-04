using Microsoft.EntityFrameworkCore;
using Webport.ERP.Inventory.Application.Data;
using Webport.ERP.Inventory.Domain.Entities.Category;
using Webport.ERP.Inventory.Domain.Entities.Item;
using Webport.ERP.Inventory.Infrastructure.Common;

namespace Webport.ERP.Inventory.Infrastructure.Database;

public sealed class InventoryDbContext(
    DbContextOptions<InventoryDbContext> options,
    TenantProvider tenantProvider) : DbContext(options), IInventoryDbContext
{
    private readonly int _tenantId = tenantProvider.TenantId;

    public DbSet<CategoryM> Categories => Set<CategoryM>();
    public DbSet<ItemM> Items => Set<ItemM>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // Query Filters
        modelBuilder.Entity<CategoryM>()
            .HasQueryFilter(_ => _.TenantId == _tenantId);

        modelBuilder.Entity<ItemM>()
            .HasQueryFilter(_ => _.TenantId == _tenantId);

        // Schema
        modelBuilder.HasDefaultSchema(InventoryConstants.Schema);

        // Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
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

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = _tenantId;
                    break;
            }
        }
        var result = base.SaveChanges();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = _tenantId;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}