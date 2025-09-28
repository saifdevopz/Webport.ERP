using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Webport.ERP.Common.Application.Authorization;
using Webport.ERP.Common.Application.Messaging;
using Webport.ERP.Common.Presentation.Endpoints;
using Webport.ERP.Identity.Application.Interfaces;
using Webport.ERP.Identity.Domain;
using Webport.ERP.Identity.Infrastructure.Common;
using Webport.ERP.Identity.Infrastructure.Database;
using Webport.ERP.Identity.Infrastructure.Database.DataAccess;
using Webport.ERP.Identity.Infrastructure.Outbox;
using Webport.ERP.Identity.Infrastructure.Services;

namespace Webport.ERP.Identity.Infrastructure;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration,
        string identityDatabaseString)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDomainEventHandlers();

        services.AddInfrastructure(configuration, identityDatabaseString);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string systemDatabaseString)
    {
        services.AddScoped<DataSeeder>();

        services.AddScoped(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPermissionService, PermissionService>();

        services.AddDbContext<IdentityDbContext>((sp, options) =>
        {
            options.UseNpgsql(systemDatabaseString, npgsqlOptionsAction =>
            {
                npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(2),
                        errorCodesToAdd: null);

                npgsqlOptionsAction.MigrationsHistoryTable(HistoryRepository.DefaultTableName, IdentityConstants.Schema);
            })
            .UseSnakeCaseNamingConvention();
        });

        services.Configure<OutboxOptions>(configuration.GetSection("Events:Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {
        Type[] domainEventHandlers = [.. Application.AssemblyReference.Assembly
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
