using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using Sentry.OpenTelemetry;
using Webport.ERP.Common.Application.Database;
using Webport.ERP.Common.Infrastructure.Authentication;
using Webport.ERP.Common.Infrastructure.Authorization;
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
        services.AddAuthorizationInternal();

        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();
        services.TryAddSingleton<AuditableEntityInterceptor>();

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        //Quartz
        services.AddQuartz(configurator =>
        {
            Guid scheduler = Guid.NewGuid();
            configurator.SchedulerId = $"default-id-{scheduler}";
            configurator.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        // OpenTelemetry

        services.AddOpenTelemetry()
                .ConfigureResource(_ => _.AddService(serviceName))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddNpgsqlInstrumentation();
                    //.AddOtlpExporter(options =>
                    //{
                    //    //var otelConfig = configuration.GetSection("OpenTelemetry");

                    //    //options.Endpoint = new Uri(otelConfig["Endpoint"]!);
                    //    //options.Headers = otelConfig["Headers"]!;
                    //    //options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    //});

                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddEntityFrameworkCoreInstrumentation()         
                        .AddNpgsql()
                        .AddSentry();

                    //tracing.AddOtlpExporter(options =>
                    //    {
                    //        //var otelConfig = configuration.GetSection("OpenTelemetry");

                    //        //options.Endpoint = new Uri(otelConfig["Endpoint"]!);
                    //        //options.Headers = otelConfig["Headers"]!;
                    //        //options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
                    //    });

                })
                .UseOtlpExporter();

        return services;
    }
}
