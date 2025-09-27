using Microsoft.EntityFrameworkCore;
using Webport.ERP.Identity.Infrastructure.Database;
using Webport.ERP.Identity.Infrastructure.Database.DataAccess;

namespace Webport.ERP.Api.Extensions;

internal static class MigrationExtensions
{
    public static async Task ApplyAllMigrations(this IApplicationBuilder app)
    {
        await app.ApplyIdentityMigrations();
        await app.ApplySystemSeeder();
    }

    public static async Task ApplyIdentityMigrations(this IApplicationBuilder app)
    {
        app.ApplyCustomMigration<IdentityDbContext>(null);
        await Task.CompletedTask;
    }

    private static void ApplyCustomMigration<TDbContext>(this IApplicationBuilder app, string? connectionString)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();

        if (connectionString != null)
        {
            context.Database.SetConnectionString(connectionString);
        }

        if (context.Database.GetPendingMigrations().Any())
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Applying Migrations for '{connectionString ?? "System Database"}'.");
            Console.ResetColor();
            context.Database.Migrate();
        }
    }

    public static async Task ApplySystemSeeder(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        DataSeeder seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
    }
}