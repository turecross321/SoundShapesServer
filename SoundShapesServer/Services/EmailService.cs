using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Services;
using NotEnoughLogs;
using System.Net.Mail;
using SoundShapesServer.Configuration;


namespace SoundShapesServer.Services;

public class EmailService : EndpointService
{
    private readonly SmtpClient _smtpClient;
    
    private readonly GameServerConfig _config;
    
    internal EmailService(LoggerContainer<BunkumContext> logger, GameServerConfig config) : base(logger)
    {
        _config = config;
        
        _smtpClient = new SmtpClient(_config.EmailHost)
        {
            Port = config.EmailHostPort,
            Credentials = new NetworkCredential(_config.EmailAddress, _config.EmailPassword),
            EnableSsl = _config.EmailSsl
        };
    }

    public void SendEmail(string recipient, string subject, string body)
    {
        MailMessage message = new();
        message.From = new MailAddress(_config.EmailAddress);
        message.To.Add(recipient);
        message.Subject = subject;
        message.Body = body;

        _smtpClient.Send(message);
    }
}
