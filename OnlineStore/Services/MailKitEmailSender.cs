using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using OnlineStore.Configuration;
using OnlineStore.Interface;

namespace OnlineStore.Services;

public class MailKitEmailSender : IEmailSender, IAsyncDisposable
{
    private readonly ILogger<MailKitEmailSender> _logger;
    private readonly SmtpConfig                  _config;
    private readonly SmtpClient                  _client;

    public MailKitEmailSender(
        IOptionsSnapshot<SmtpConfig> options, 
        ILogger<MailKitEmailSender> logger
        )
    {
        _logger = logger;
        _config = options.Value;
        _client = new SmtpClient();
    }

    public async Task SendAsync(
        string fromName,
        string to,
        string subject,
        string bodyHtml
        ) 
    {
        _logger.LogInformation("Trying to send a letter");
        
        var message = new MimeMessage();
        var fromEmail = _config.UserNAme;
        
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = bodyHtml };
        
        if (!_client.IsConnected)
        {
            await _client.ConnectAsync(_config.Host, _config.Port, _config.UseSsl);
        }
        
        if (!_client.IsAuthenticated)
        {
            await _client.AuthenticateAsync(_config.UserNAme, _config.Password);
        }
        
        await _client.SendAsync(message);
    }

    public async ValueTask DisposeAsync()
    {
        await _client.DisconnectAsync(true);
        _client.Dispose();
    }
}