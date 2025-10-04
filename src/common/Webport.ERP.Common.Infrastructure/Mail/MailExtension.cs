using Microsoft.Extensions.DependencyInjection;
using Webport.ERP.Common.Application.Mail;

namespace Webport.ERP.Common.Infrastructure.Mail;

internal static class MailExtension
{
    internal static IServiceCollection ConfigureMailing(this IServiceCollection services)
    {
        services.AddTransient<IMailService, MailService>();
        services.AddOptions<MailOptions>().BindConfiguration(nameof(MailOptions));

        return services;
    }
}