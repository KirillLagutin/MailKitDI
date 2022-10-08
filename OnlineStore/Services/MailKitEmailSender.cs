using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using OnlineStore.Interface;

namespace OnlineStore.Services;

public class MailKitEmailSender : IEmailSender
{
    public void Send(
        string fromName, 
        string fromEmail, 
        string to, 
        string subject, 
        string bodyHtml
        ) {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart(TextFormat.Html) { Text = bodyHtml };

        using var client = new SmtpClient();
        client.Connect("smtp.beget.com", 25, false);

        // Note: only needed if the SMTP server requires authentication
        client.Authenticate("asp2022pd011@rodion-m.ru", "6WU4x2be");

        client.Send(message);
        client.Disconnect(true);
    }
}