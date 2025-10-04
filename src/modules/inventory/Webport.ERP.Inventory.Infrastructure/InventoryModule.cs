using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webport.ERP.Inventory.Application.Interfaces;
using Webport.ERP.Inventory.Infrastructure.Common;
using Webport.ERP.Inventory.Infrastructure.Database;
using Webport.ERP.Inventory.Infrastructure.Database.DataAccess;
using Webport.ERP.Inventory.Infrastructure.Outbox;
using Webport.ERP.Inventory.Presentation;

namespace Webport.ERP.Inventory.Infrastructure;

public static class InventoryModule
{
    public static IServiceCollection AddInventoryModule(
    this IServiceCollection services,
    IConfiguration configuration,
    string tenantDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration, tenantDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string identityDatabaseString)
    {
        services.AddScoped<TenantProvider>();


        services.AddScoped(typeof(IInventoryRepository<>), typeof(InventoryRepository<>));

        services.AddDbContext<InventoryDbContext>((sp, options) =>
        {
            options.UseNpgsql(identityDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, InventoryConstants.Schema);
            })
            .UseSnakeCaseNamingConvention();
        });

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }

    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = [.. AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventDispatcher)))];

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);

            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}
