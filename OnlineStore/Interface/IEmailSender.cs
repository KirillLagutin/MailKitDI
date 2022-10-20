namespace OnlineStore.Interface;

public interface IEmailSender
{
    Task SendAsync(
        string fromName,
        string to,
        string subject,
        string bodyHtml);
}