using Scalar.AspNetCore;
using System.Reflection;
using Webport.ERP.Api.Extensions;
using Webport.ERP.Common.Application;
using Webport.ERP.Common.Infrastructure;
using Webport.ERP.Common.Infrastructure.Middlewares;
using Webport.ERP.Common.Presentation.Endpoints;
using Webport.ERP.Identity.Infrastructure;
using Webport.ERP.Inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Connection Strings
string? identityDbString = builder.Configuration["PostgreSQL:IdentityConnection"];
ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);

string? tenantDbString = builder.Configuration["PostgreSQL:TenantConnection"];
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
builder.Services.AddCommonInfrastructure(builder.Configuration, "Webport.Name");

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
