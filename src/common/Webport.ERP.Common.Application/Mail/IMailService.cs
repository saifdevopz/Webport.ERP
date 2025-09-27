namespace Webport.ERP.Common.Application.Mail;

public interface IMailService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}