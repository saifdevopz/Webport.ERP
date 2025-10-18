using Serilog;
using Webport.ERP.Api.Extensions;
using Webport.ERP.Common.Infrastructure.Logging;

StaticLogger.EnsureInitialized();
Log.Information("Server booting up...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureSystemConfigurations();

    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        await app.ApplyAllMigrations();
    }

    app.UseSystemConfigurations();

    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception during startup.");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server shutting down...");
    await Log.CloseAndFlushAsync();
}