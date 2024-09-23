using System.Net;
using System.Net.Mail;
using Bunkum.Core;
using Bunkum.Core.Services;
using NotEnoughLogs;
using SoundShapesServer.Types.Config;

namespace SoundShapesServer.Services;

public class EmailService : EndpointService
{
    private readonly ServerConfig _config;
    
    internal EmailService(Logger logger, ServerConfig config) : base(logger)
    {
        this._config = config;
    }

    public bool SendEmail(string recipient, string subject, string html)
    {
        using SmtpClient smtpClient = new SmtpClient(this._config.EmailSettings.Host);
        smtpClient.Port = this._config.EmailSettings.HostPort;
        smtpClient.Credentials = new NetworkCredential(this._config.EmailSettings.Address, this._config.EmailSettings.Password);
        smtpClient.EnableSsl = this._config.EmailSettings.UseSsl;

        MailMessage message = new();
        message.From = new MailAddress(this._config.EmailSettings.Address);
        message.To.Add(recipient);
        message.Subject = subject;
        message.Body = html;
        message.IsBodyHtml = true;

        try
        {
            smtpClient.Send(message);
        }
        catch (Exception e)
        {
            this.Logger.LogWarning(BunkumCategory.Service, $"Failed to send '{subject}' to '{recipient}':\n{e}");
            return false;
        }

        return true;
    }
}