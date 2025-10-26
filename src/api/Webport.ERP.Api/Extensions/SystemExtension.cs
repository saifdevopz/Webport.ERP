using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using Webport.ERP.Common.Application;
using Webport.ERP.Common.Infrastructure;
using Webport.ERP.Common.Infrastructure.Middlewares;
using Webport.ERP.Common.Presentation.Endpoints;
using Webport.ERP.Identity.Infrastructure;
using Webport.ERP.Inventory.Infrastructure;
using Webport.ERP.ServiceDefaults;

namespace Webport.ERP.Api.Extensions;

public static class SystemExtension
{
    public static WebApplicationBuilder ConfigureSystemConfigurations(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // DEVELOPMENT SETTINGS

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.AddServiceDefaults();

        // --- Database Configuration (Dynamic, Multi-Cloud Support) ---
        var config = builder.Configuration;

        string provider = config["Database:ActiveProvider"]
            ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

        string cloud = config["Database:ActiveCloud"]
            ?? throw new ArgumentException("Missing Database:ActiveCloud in configuration.");

        string basePath = $"Database:Providers:{provider}:{cloud}";

        // Fetch connection strings dynamically
        string? identityDbString = config[$"{basePath}:IdentityConnection"];
        string? tenantDbString = config[$"{basePath}:TenantConnection"];

        // Optional fallback for Aspire local development
        if (string.IsNullOrWhiteSpace(identityDbString) || string.IsNullOrWhiteSpace(tenantDbString))
        {
            var localDevConnection = config.GetConnectionString("demo-db");
            if (!string.IsNullOrWhiteSpace(localDevConnection))
            {
                identityDbString ??= localDevConnection;
                tenantDbString ??= localDevConnection;
            }
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantDbString);

        builder.ConfigureDatabaseConfigurations();

        // --- MVC & API ---
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        // --- Exception Handling ---
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        // --- Application Modules ---
        Assembly[] moduleApplicationAssemblies =
        [
            Webport.ERP.Identity.Application.AssemblyReference.Assembly,
            Webport.ERP.Inventory.Application.AssemblyReference.Assembly,
        ];

        // --- Common Modules ---
        builder.Services.AddCommonApplication(moduleApplicationAssemblies);
        builder.Services.AddCommonInfrastructure(builder.Configuration);

        // --- Infrastructure Modules with Dynamic Connection Strings ---
        builder.Services.AddIdentityModule(builder.Configuration, identityDbString);
        builder.Services.AddInventoryModule(builder.Configuration, tenantDbString);

        // --- CORS ---
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return builder;
    }

    public static WebApplication UseSystemConfigurations(this WebApplication app)
    {
        app.UseCors("MyPolicy");

        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(_ =>
            {
                _.Servers = [];
                _.Theme = ScalarTheme.Kepler;
            });
        }

        app.UseSerilogRequestLogging();

        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.MapEndpoints();

        return app;
    }
}