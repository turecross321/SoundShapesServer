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
using SoundShapesServer.Types.ServerResponses.Game;
using static System.Net.HttpStatusCode;

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
            return BadRequest;
        }
        
        DbUser user = database.GetUserWithName(ticket.Username) ?? database.CreateUser(ticket.Username);
        DbToken token;

        if (!user.FinishedRegistration)
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"Creating eula token ({platform}) for  {user}.");
            token = database.CreateToken(user, TokenType.GameEula, platform);
        }
        else
        {
            context.Logger.LogInfo(BunkumCategory.Authentication, $"Creating access token ({platform}) for {user}.");
            token = database.CreateToken(user, TokenType.GameAccess, platform);
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
                    UserName = ticket.Username
                }
            }
        };

        return new Response(response, ContentType.Json, Created);
    }
    
    [GameEndpoint("~identity:*.hello"), Authentication(false)]
    public Response Hello(RequestContext context)
    {
        return OK;
    }
}