using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;
using ContentType = Bunkum.Listener.Protocol.ContentType;
using SoundShapesServer.Configuration;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServer.Verification;
using static SoundShapesServer.Helpers.IpHelper;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [GameEndpoint("identity/login/token/psn.post", ContentType.Json, HttpMethods.Post)]
    [HttpEndpoint("/identity/login/token/psn", ContentType.Json, HttpMethods.Post)]
    [Authentication(false)]
    public Response? LogIn(RequestContext context, GameDatabaseContext database, Stream body, GameServerConfig config, IDataStore dataStore)
    {
        Ticket ticket;
        try
        {
            ticket = Ticket.ReadFromStream(body);
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(BunkumCategory.Authentication, "Could not read ticket: " + e);
            return HttpStatusCode.BadRequest;
        }

        if (!UserHelper.IsUsernameLegal(ticket.Username)) return HttpStatusCode.BadRequest;

        GameUser? user = database.GetUserWithUsername(ticket.Username, true);
        if (user == null && !config.AccountCreation)
        {
            return new Response(HttpStatusCode.Created);
        }
        user ??= database.CreateUser(ticket.Username);

        PlatformType platformType = PlatformHelper.GetPlatformType(ticket);
        GameIp gameIp = GetGameIpFromRequestContext(context, database, user);
        bool genuineTicket = VerifyTicket(context, (MemoryStream)body, ticket);

        SessionType? sessionType;

        if (config.RequireAuthentication)
        {
            if (!user.HasFinishedRegistration)
            {
                sessionType = SessionType.GameUnAuthorized;
            }
            else if (genuineTicket && ((platformType is PlatformType.Ps3 or PlatformType.PsVita && user.AllowPsnAuthentication) || 
                                       (platformType is PlatformType.Rpcs3 && user.AllowRpcnAuthentication)))
            {
                sessionType = SessionType.Game;
            }
            else if (gameIp.Authorized)
            {
                sessionType = SessionType.Game;
                if (gameIp.OneTimeUse) 
                    database.UseOneTimeIpAddress(gameIp);
            }
            else
            {
                sessionType = SessionType.GameUnAuthorized;
            }
        }
        else
        {
            sessionType = SessionType.Game;   
        }

        if (GetActiveUserBans(user).Any()) 
            sessionType = SessionType.Banned;
        if (user.Deleted) 
            sessionType = SessionType.GameUnAuthorized;
        
        GameSession session = database.CreateSession(user, (SessionType)sessionType, platformType, genuineTicket, Globals.FourHoursInSeconds);

        GameSessionResponse sessionResponse = new (session);
        GameSessionWrapper responseWrapper = new (sessionResponse);

        context.Logger.LogInfo(BunkumCategory.Authentication, $"{sessionResponse.User.Username} has logged in.");

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={sessionResponse.Id};Version=1;Path=/");
        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }
    
    private static bool VerifyTicket(RequestContext context, MemoryStream body, Ticket ticket)
    {
        ITicketSigningKey signingKey;

        // Determine the correct key to use
        if (ticket.IssuerId == 0x33333333)
        {
            context.Logger.LogDebug(BunkumCategory.Authentication, "Using RPCN ticket key");
            signingKey = RpcnSigningKey.Instance;
        }
        else
        {
            context.Logger.LogDebug(BunkumCategory.Authentication, "Using PSN Sound Shapes ticket key");
            signingKey = SoundShapesSigningKey.Instance;
        }
        
        TicketVerifier verifier = new(body.ToArray(), ticket, signingKey);
        return verifier.IsTicketValid();
    }
    
    [GameEndpoint("~identity:*.hello"), Authentication(false)]
    public Response Hello(RequestContext context)
    {
        return HttpStatusCode.OK;
    }
    
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get"), Authentication(false)]
    public string? Eula(RequestContext context, GameServerConfig config, GameDatabaseContext database, string platform, string publisher, string language, GameSession? session, GameUser? user)
    {
        if (session?.SessionType == SessionType.Game)
            return EulaEndpoint.NormalEula(config);
        
        string eulaEnd = $"\n \n{DateTime.UtcNow}";

        if (user == null)
        {
            if (!config.AccountCreation)
                return "Account Creation is disabled on this instance." + eulaEnd;
            return null;
        }
        
        if (user.Deleted)
            return $"The account attached to your username ({user.Username}) has been deleted, and is no longer available." + eulaEnd;
        
        IQueryable<Punishment> bans = GetActiveUserBans(user);
        if (bans.Any())
        {
            Punishment longestBan = bans.Last();
            
            return "You are banned.\n" +
                   "Expires at " + longestBan.ExpiryDate.Date + ".\n" + 
                   "Reason: \"" + longestBan.Reason + "\"" + eulaEnd;
        }
        if (user.HasFinishedRegistration == false)
        {
            string emailSessionId = GenerateEmailSessionId(database);
            database.CreateSession(user, SessionType.SetEmail, PlatformType.Api, null, Globals.TenMinutesInSeconds, emailSessionId);
            return $"Your account is not registered.\n \n" +
                   $"To proceed, you will have to register an account at {config.WebsiteUrl}/register\n" +
                   $"Your email code is: {emailSessionId}" + eulaEnd;
        }
        
        string unAuthorizedBase = $"Your session has not been authenticated.\n \n" +
                             $"To proceed, you will have to log into your account at {config.WebsiteUrl}/authentication " +
                             $"and perform one of the following actions:\n";
                
        List<string> authorizationMethods = new ();

        if (session!.GenuineNpTicket == true)
        {
            switch (session.PlatformType)
            {
                case PlatformType.Ps3 or PlatformType.PsVita when !user.AllowPsnAuthentication:
                    authorizationMethods.Add("Enable PSN Authentication.");
                    break;
                case PlatformType.Rpcs3 when !user.AllowRpcnAuthentication:
                    authorizationMethods.Add("Enable RPCN Authentication.");
                    break;
            }
        }

        authorizationMethods.Add(user.AllowIpAuthentication
            ? "Authorize your IP address."
            : "Enable IP Authentication and authorize your IP address. (Note: You will have to reconnect upon enabling IP Authentication for your IP to show up.)");

        string formattedMethods = authorizationMethods.Aggregate("", (current, method) => current + ("- " + method + "\n"));
        return unAuthorizedBase + formattedMethods + eulaEnd;
    }
}