using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;
using SoundShapesServer.Configuration;
using SoundShapesServer.Helpers;
using static SoundShapesServer.Helpers.IpHelper;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [Endpoint("/identity/login/token/psn", ContentType.Json, Method.Post)]
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
            return HttpStatusCode.BadRequest;
        }
        
        if (!UserHelper.IsUsernameLegal(ticket.Username)) return HttpStatusCode.BadRequest;

        GameUser? user = database.GetUserWithUsername(ticket.Username);
        user ??= database.CreateUser(ticket.Username);

        GameSession? session = null;
        IpAuthorization ip = GetIpAuthorizationFromRequestContext(context, database, user, SessionType.Game);

        if (config.ApiAuthentication)
        {
            // If user hasn't finished registration, or if their IP isn't authorized, give them an unauthorized Session
            if (user.HasFinishedRegistration == false || ip.Authorized == false)
            {
                session = database.GenerateSessionForUser(context, user, SessionType.Unauthorized, 30);
            }
        }
        
        // Check if user is banned
        Punishment[] bans = GetUsersPunishmentsOfType(user, PunishmentType.Ban);
        if (bans.Length > 0) session = database.GenerateSessionForUser(context, user, SessionType.Unauthorized, 30);

        session ??= database.GenerateSessionForUser(context, user, (int)SessionType.Game, 14400); // 4 hours
        
        if (session.Ip.OneTimeUse) database.UseIpAddress(session.Ip);

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
    public string? Eula(RequestContext context, GameServerConfig config, RealmDatabaseContext database, string platform, string publisher, string language, GameSession session, GameUser user)
    {
        if (session.SessionType != (int)SessionType.Unauthorized)
            return EulaEndpoint.NormalEula(config);
        
        // Check if user is banned
        Punishment[] bans = GetUsersPunishmentsOfType(user, PunishmentType.Ban);
        if (bans.Length > 0)
        {
            Punishment? longestBan = bans.MaxBy(p => p.ExpiresAt);
            if (longestBan == null) return "User is banned.";
            
            string banString = "User is banned. Expires at " + longestBan.ExpiresAt + ".\n" +
                               "Reason: " + longestBan.Reason;
            
            return banString;
        }

        if (user.HasFinishedRegistration == false)
        {
            string emailSessionId = GenerateEmailSessionId(database);
            database.GenerateSessionForUser(context, user, SessionType.SetEmail, 600, emailSessionId); // 10 minutes
            return $"Your account is not registered. To proceed, you will have to register an account at {config.WebsiteUrl}.\nYour email code is: {emailSessionId}\n-\n{DateTime.UtcNow}";
        }

        if (session.Ip.Authorized == false)
            return $"Your IP Address has not been authorized. To proceed, you will have to log in to your account at {config.WebsiteUrl} and authorize the following IP Address: {session.Ip.IpAddress}\n-\n{DateTime.UtcNow}";

        return null;
    }
    
}