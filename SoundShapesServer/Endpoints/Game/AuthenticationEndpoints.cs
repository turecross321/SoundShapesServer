using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.AuthenticationHelper;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;
using SoundShapesServer.Configuration;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [Endpoint("/identity/login/token/psn", ContentType.Json, Method.Post)]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    [Authentication(false)]
    public Response? Login(RequestContext context, RealmDatabaseContext database, Stream body, GameServerConfig config)
    {
        Ticket ticket;
        try
        {
            ticket = Ticket.FromStream(body);
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(BunkumContext.Authentication, "Could not read ticket: " + e);
            return null;
        }

        bool rpcs3;

        switch (ticket.IssuerId)
        { 
            case 0x100: // ps3, ps4, psvita
                rpcs3 = false; 
                break;
            case 0x33333333: // rpcs3
                rpcs3 = true; 
                break;
            default: // unknown
                return HttpStatusCode.Forbidden; 
        }

        TypeOfSession typeOfSession = PlatformHelper.GetSessionType(ticket.TitleId, rpcs3);
        if (typeOfSession == TypeOfSession.Unknown) return HttpStatusCode.Forbidden; 
        
        GameUser? user = database.GetUserWithUsername(ticket.Username);
        user ??= database.CreateUser(ticket.Username);

        GameSession? session = null;

        if (config.ApiAuthentication)
        {
            string ipAddress = ((IPEndPoint)context.RemoteEndpoint).Address.ToString();
        
            // If user hasn't finished registration, or if their IP isn't authorized, give them an unauthorized Session
            if (user.HasFinishedRegistration == false)
            {
                session = database.GenerateSessionForUser(user, (int)TypeOfSession.Unauthorized, 5);
            }

            // If the user's IP isn't authorized, give them an unauthorized Session and track their IP
            else if (user.AuthorizedIPAddresses.Contains(ipAddress) == false)
            {
                session = database.GenerateSessionForUser(user, (int)TypeOfSession.Unauthorized, 5);
                database.AddUnAuthenticatedIpAddress(user, ipAddress);
            }
        }

        // Otherwise, generate an actual session
        session ??= database.GenerateSessionForUser(user, (int)typeOfSession, 14400); // 4 hours
        GameSessionResponse gameSessionResponse = SessionToSessionResponse(session);

        return SessionResponseToResponse(context, gameSessionResponse);
    }

    
    [GameEndpoint("~identity:*.hello", ContentType.Json)]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public Response Hello(RequestContext context)
    {
        return HttpStatusCode.OK;
    }
    
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get", ContentType.Json)]
    public string? Eula(RequestContext context, GameServerConfig config, RealmDatabaseContext database, string platform, string publisher, string language, GameSession token, GameUser user)
    {
        if (token.SessionType != (int)TypeOfSession.Unauthorized)
            return EulaEndpoint.NormalEula(config);
        
        if (user.HasFinishedRegistration == false)
        {
            string emailSessionId = SessionHelper.GenerateSimpleSessionId(database);
            database.GenerateSessionForUser(user, (int)TypeOfSession.SetEmail, 600, emailSessionId); // 10 minutes
            return $"Your account is not registered. To proceed, you will have to register an account at {config.WebsiteUrl}.\nYour verification code is: {emailSessionId}\n-\n{DateTime.UtcNow}";
        }

        string ipAddress = ((IPEndPoint)context.RemoteEndpoint).Address.ToString();

        if (user.AuthorizedIPAddresses.Contains(ipAddress) == false)
            return $"Your IP Address has not been authorized. To proceed, you will have to log in to your account at {config.WebsiteUrl} and authorize the following IP Address: {ipAddress}\n-\n{DateTime.UtcNow}";

        return null;
    }
    
}