using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;
using SoundShapesServer.Configuration;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.IpHelper;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [GameEndpoint("identity/login/token/psn.post", ContentType.Json, Method.Post)]
    [Endpoint("/identity/login/token/psn", ContentType.Json, Method.Post)]
    [Authentication(false)]
    public Response? Login(RequestContext context, GameDatabaseContext database, Stream body, GameServerConfig config)
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

        GameUser? user = database.GetUserWithUsername(ticket.Username, true);
        user ??= database.CreateUser(ticket.Username);
        
        IpAuthorization ip = GetIpAuthorizationFromRequestContext(context, database, user);

        SessionType? sessionType = null;
        
        if (config.ApiAuthentication)
        {
            if (user.HasFinishedRegistration == false || ip.Authorized == false)
                sessionType = SessionType.GameUnAuthorized;
        }
        
        if (GetActiveUserBans(user).Any()) sessionType = SessionType.Banned;
        if (user.Deleted) sessionType = SessionType.GameUnAuthorized;

        PlatformType platformType = PlatformHelper.GetPlatformType(ticket);

        sessionType ??= SessionType.Game;
        GameSession session = database.CreateSession(user, (SessionType)sessionType, platformType, Globals.FourHoursInSeconds, null, ip);
        
        if (session.Ip is { OneTimeUse: true }) database.UseOneTimeIpAddress(session.Ip);

        GameSessionResponse sessionResponse = new (session);
        GameSessionWrapper responseWrapper = new (sessionResponse);

        context.Logger.LogInfo(BunkumContext.Authentication, $"{sessionResponse.User.Username} has logged in.");

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={sessionResponse.Id};Version=1;Path=/");
        // ReSharper disable StringLiteralTypo
        context.ResponseHeaders.Add("x-otg-identity-displayname", sessionResponse.User.Username);
        context.ResponseHeaders.Add("x-otg-identity-personid", sessionResponse.User.Id);
        context.ResponseHeaders.Add("x-otg-identity-sessionid", sessionResponse.Id);
        // ReSharper restore StringLiteralTypo
        
        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }

    
    [GameEndpoint("~identity:*.hello"), Authentication(false)]
    public Response Hello(RequestContext context)
    {
        return HttpStatusCode.OK;
    }
    
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get")]
    public string Eula(RequestContext context, GameServerConfig config, GameDatabaseContext database, string platform, string publisher, string language, GameSession session, GameUser user)
    {
        if (session.SessionType == SessionType.Game)
            return EulaEndpoint.NormalEula(config);

        string? eula = null;
        
        IQueryable<Punishment> bans = GetActiveUserBans(user);
        
        if (user.Deleted)
            eula = $"The account attached to your username ({user.Username}) has been deleted, and is no longer available.";
        else if (bans.Any())
        {
            Punishment longestBan = bans.Last();
            
            eula = "You are banned.\n" +
                   "Expires at " + longestBan.ExpiryDate.Date + ".\n" + 
                   "Reason: \"" + longestBan.Reason + "\"";
        }
        else if (user.HasFinishedRegistration == false)
        {
            IpAuthorization ip = GetIpAuthorizationFromRequestContext(context, database, user);

            string emailSessionId = GenerateEmailSessionId(database);
            database.CreateSession(user, SessionType.SetEmail, session.PlatformType, Globals.TenMinutesInSeconds, emailSessionId, ip);
            eula = $"Your account is not registered.\n" +
                   $"To proceed, you will have to register an account at {config.WebsiteUrl}/register\n" +
                   $"Your email code is: {emailSessionId}";
        }
        else if (session.Ip is { Authorized: false })
            return $"Your IP address has not been authorized.\n" +
                   $"To proceed, you will have to log into your account at {config.WebsiteUrl}/authorization " +
                   $"and approve your IP address.";

        return eula + $"\n-\n{DateTime.UtcNow}";
    }
}