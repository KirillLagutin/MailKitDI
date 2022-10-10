using OnlineStore.Services;

namespace OnlineStore.Interface;

public interface IEmailSender
{
    void Send(
        string fromName,
        string to, 
        string subject, 
        string bodyHtml
    );
}