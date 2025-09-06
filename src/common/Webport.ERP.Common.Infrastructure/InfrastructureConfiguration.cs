using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;
using Webport.ERP.Common.Application.Database;
using Webport.ERP.Common.Infrastructure.Authentication;
using Webport.ERP.Common.Infrastructure.Clock;
using Webport.ERP.Common.Infrastructure.Database;
using Webport.ERP.Common.Infrastructure.Interceptors;
using Webport.ERP.Common.Infrastructure.Mail;

namespace Webport.ERP.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddCommonInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        // Mail
        services.ConfigureMailing();

        services.AddAuthenticationInternal();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        //Quartz
        services.AddQuartz(configurator =>
        {
            Guid scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
