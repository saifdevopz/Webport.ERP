using Serilog;
using Serilog.Core;
using System.Globalization;

namespace Webport.ERP.Common.Infrastructure.Logging;

public static class StaticLogger
{
    public static void EnsureInitialized()
    {
        if (Log.Logger is not Logger)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                  .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                .CreateLogger();
        }
    }
}