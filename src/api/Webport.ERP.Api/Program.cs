using Scalar.AspNetCore;
using Sentry.OpenTelemetry;
using System.Reflection;
using Webport.ERP.Api.Extensions;
using Webport.ERP.Common.Application;
using Webport.ERP.Common.Infrastructure;
using Webport.ERP.Common.Infrastructure.Middlewares;
using Webport.ERP.Common.Presentation.Endpoints;
using Webport.ERP.Identity.Infrastructure;
using Webport.ERP.Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// PRODUCTION

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

// Database Connection Strings

string? databaseType = builder.Configuration["DatabaseType:Active"];
string? identityDbString = null;
string? tenantDbString = null;

if (string.IsNullOrWhiteSpace(databaseType))
{
    throw new ArgumentException("Database type is not configured (DatabaseType:Active).");
}

switch (databaseType.Trim().ToUpperInvariant())
{
    case "POSTGRESQL":
        identityDbString = builder.Configuration["PostgreSQL:IdentityConnection"];
        tenantDbString = builder.Configuration["PostgreSQL:TenantConnection"];
        break;

    case "SQLSERVER":
        identityDbString = builder.Configuration["SQLServer:IdentityConnection"];
        tenantDbString = builder.Configuration["SQLServer:TenantConnection"];
        break;

    default:
        throw new ArgumentException($"Unsupported database type: {databaseType}");
}

// Validate connection strings
ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);
ArgumentException.ThrowIfNullOrWhiteSpace(tenantDbString);

// Controller Support
builder.Services.AddControllers();

// Minimal API Support
builder.Services.AddEndpointsApiExplorer();

// Open API
builder.Services.AddOpenApi();

// Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Application Module Assemblies
Assembly[] moduleApplicationAssemblies =
[
    Webport.ERP.Identity.Application.AssemblyReference.Assembly,
    Webport.ERP.Inventory.Application.AssemblyReference.Assembly,
];

// Common Application Module
builder.Services.AddCommonApplication(moduleApplicationAssemblies);

// Common Infrastructure Module
builder.Services.AddCommonInfrastructure(builder.Configuration, "Webport.ERP");

// Identity and Inventory Infrastructure Modules
builder.Services.AddIdentityModule(builder.Configuration, identityDbString);
builder.Services.AddInventoryModule(builder.Configuration, tenantDbString);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("MyPolicy");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ =>
    {
        _.Servers = [];
        _.Theme = ScalarTheme.Kepler;
    });

    await app.ApplyAllMigrations();
}

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();
