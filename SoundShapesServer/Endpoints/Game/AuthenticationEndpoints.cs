using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using NPTicket;
using SoundShapesServer.Common.Verification;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Game;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [Authentication(false)]
    [GameEndpoint("identity/login/token/psn.post", HttpMethods.Post)]
    public Response Authenticate(RequestContext context, GameDatabaseContext database, ServerConfig config, Stream body)
    {
        Ticket ticket;
        try
        {
            ticket = Ticket.ReadFromStream(body);
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(BunkumCategory.Authentication, $"Could not read ticket: \"{e}\".");
            return BadRequest;
        }

        if (!CommonPatterns.UsernameRegex().IsMatch(ticket.Username))
        {
            context.Logger.LogWarning(BunkumCategory.Authentication, $"Invalid username: \"{ticket.Username}\".");
            return BadRequest;
        }

        PlatformType? platform = ticket.GetPlatformType();
        if (platform == null)
        {
            context.Logger.LogWarning(BunkumCategory.Authentication, $"Unable to determine PlatformType ({ticket.Username}).");
            // todo: notification about unable to determine platform type
            return BadRequest;
        }
        
        DbUser user = database.GetUserWithName(ticket.Username) ?? database.CreateUser(ticket.Username);
        DbToken token;

        // todo: check for bans

        bool allowAuthentication = false;
        DbIp? ip = null; // required for IP authentication

        bool genuineTicket = ticket.IsGenuine((MemoryStream)body, database.Time.Now, platform);
        
        if (genuineTicket)
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} has a genuine ticket.");
            if (platform is PlatformType.PS3 or PlatformType.PS4 or PlatformType.PSVita
                && user.PsnAuthorization)
                allowAuthentication = true;
            if (user.RpcnAuthorization && platform is PlatformType.RPCS3)
                allowAuthentication = true;
        }
        else
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} doesn't have a genuine ticket.");
        }

        if (user.IpAuthorization)
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"Tracking IP address for {user} as " +
                                                                  $"they have enabled IP authentication.");
            ip = database.GetOrCreateIp(user, ((IPEndPoint)context.RemoteEndpoint).Address.ToString());
            
            if (ip.Authorized)
            {
                allowAuthentication = true;
                context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} has an authorized IP address.");
            }
        }

        if (!user.FinishedRegistration)
        {
            allowAuthentication = false;
        }
        
        if (allowAuthentication)
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"Creating access token ({platform}) for {user}.");
            token = database.CreateToken(user, TokenType.GameAccess, platform, ip, null, genuineTicket);
        }
        else
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"Creating eula token ({platform}) for  {user}.");
            token = database.CreateToken(user, TokenType.GameEula, platform, ip, null, genuineTicket);
        }
        
        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={token.Id};Version=1;Path=/");

        AuthenticationResponse response = new()
        {
            Session = new SessionResponse
            {
                Id = token.Id,
                User = new SessionUserResponse
                {
                    Id = token.UserId,
                    UserName = ticket.Username,
                },
            },
        };

        return new Response(response, ContentType.Json, Created);
    }
    
    [GameEndpoint("~identity:*.hello"), Authentication(false)]
    public Response Hello(RequestContext context)
    {
        return OK;
    }
}