using System.Net;
using Bunkum.Core.Services;
using System.Net.Mail;
using Bunkum.Core;
using NotEnoughLogs;
using SoundShapesServer.Configuration;


namespace SoundShapesServer.Services;

public class EmailService : EndpointService
{
    private readonly SmtpClient _smtpClient;
    
    private readonly GameServerConfig _config;
    
    internal EmailService(Logger logger, GameServerConfig config) : base(logger)
    {
        _config = config;
        
        _smtpClient = new SmtpClient(_config.EmailHost)
        {
            Port = config.EmailHostPort,
            Credentials = new NetworkCredential(_config.EmailAddress, _config.EmailPassword),
            EnableSsl = _config.EmailSsl
        };
    }

    public bool SendEmail(string recipient, string subject, string body)
    {
        MailMessage message = new();
        message.From = new MailAddress(_config.EmailAddress);
        message.To.Add(recipient);
        message.Subject = subject;
        message.Body = body;

        try
        {
            _smtpClient.Send(message);
        }
        catch (Exception e)
        {
            Logger.LogWarning(BunkumCategory.Service, $"Failed to send '{subject}' to '{recipient}':\n{e}");
            return false;
        }

        return true;
    }
}
