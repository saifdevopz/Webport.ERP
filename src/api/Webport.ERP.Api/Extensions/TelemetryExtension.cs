using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sentry.OpenTelemetry;

namespace Webport.ERP.Api.Extensions;


public static class TelemetryExtension
{
    public static WebApplicationBuilder ConfigureTelemetryConfigurations(this WebApplicationBuilder builder)
    {

        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = "";
            // Enable logs to be sent to Sentry
            options.SendDefaultPii = true;
            options.SampleRate = 1.0f;
            options.TracesSampleRate = 1.0;
            options.UseOpenTelemetry();
        });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;

            //var otelConfig = builder.Configuration.GetSection("OpenTelemetry");

            //logging.AddOtlpExporter(options =>
            //{
            //    options.Endpoint = new Uri(otelConfig["Endpoint"]!);
            //    options.Headers = otelConfig["Headers"]!;
            //    options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            //});
        });

        builder.Services.AddOpenTelemetry()
                .ConfigureResource(_ => _.AddService("Webport.ERP"))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation();
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

        return builder;
    }
}