using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using OnlineStore.Interface;

namespace OnlineStore.Services;

public class MailKitEmailSender : IEmailSender, IDisposable
{
    private readonly SmtpClient _client;

    public MailKitEmailSender()
    {
        _client = new SmtpClient();
    }

    public void Send(
        string fromName,
        string to, 
        string subject, 
        string bodyHtml
        ) {
        var message = new MimeMessage();
        var fromEmail = "asp2022pd011@rodion-m.ru";
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = bodyHtml };
        if (!_client.IsConnected)
        {
            _client.Connect("smtp.beget.com", 25, false);
        }
        if (!_client.IsAuthenticated)
        {
            _client.Authenticate("asp2022pd011@rodion-m.ru", "6WU4x2be");
        }
        _client.Send(message);
    }

    public void Dispose()
    {
        _client.Disconnect(true);
        _client.Dispose();
    }
    
}