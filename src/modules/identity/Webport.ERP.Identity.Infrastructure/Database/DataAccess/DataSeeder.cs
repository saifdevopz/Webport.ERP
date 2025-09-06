using Microsoft.EntityFrameworkCore;

namespace Webport.ERP.Identity.Infrastructure.Database.DataAccess;

public class DataSeeder(IdentityDbContext context)
{
    private readonly IdentityDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task SeedAsync()
    {
        await SeedTenantsAsync();
        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedPermissionsAsync();
        await SeedRolePermissionsAsync();
    }

    private async Task SeedTenantsAsync()
    {
        if (await _context.Tenants.AnyAsync())
        {
            return;
        }

        TenantM[] tenants =
        [
            TenantM.Create("Webport"),
            TenantM.Create("Customer1"),
            TenantM.Create("Customer2"),
        ];

        await _context.Tenants.AddRangeAsync(tenants);
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (await _context.Roles.AnyAsync())
        {
            return;
        }

        RoleM[] roles =
        [
            RoleM.Create("Admin"),
            RoleM.Create("Customer")
        ];

        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        if (await _context.Users.AnyAsync())
        {
            return;
        }

        UserM[] users =
        [
            UserM.Create(1, 1, "Saif Khan", "admin@gmail.com", "12345678"),
            UserM.Create(2, 2, "Itachi Uchiha", "customer1@gmail.com", "12345678"),
            UserM.Create(3, 2, "Naruto Uzumaki", "customer2@gmail.com", "12345678"),
        ];

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }

    private async Task SeedRolePermissionsAsync()
    {
        if (await _context.RolePermissions.AnyAsync())
        {
            return;
        }

        RolePermissionM[] rolePermissions =
        [
            new RolePermissionM { RoleId = 1, PermissionId = 1 },
            new RolePermissionM { RoleId = 2, PermissionId = 2 },
            new RolePermissionM { RoleId = 2, PermissionId = 3 }
        ];

        await _context.RolePermissions.AddRangeAsync(rolePermissions);
        await _context.SaveChangesAsync();
    }

    private async Task SeedPermissionsAsync()
    {
        if (await _context.Permissions.AnyAsync())
        {
            return;
        }

        PermissionM[] permissions =
        [
            PermissionM.Create("global:access"),
            PermissionM.Create("parent:modify"),
            PermissionM.Create("parent:read"),
        ];

        await _context.Permissions.AddRangeAsync(permissions);
        await _context.SaveChangesAsync();
    }
}