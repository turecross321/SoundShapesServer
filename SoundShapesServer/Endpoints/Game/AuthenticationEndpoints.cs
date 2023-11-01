using System.Net;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses;
using Bunkum.Core.Storage;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using NPTicket;
using NPTicket.Verification;
using NPTicket.Verification.Keys;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Configuration;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Authentication;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;
using SoundShapesServer.Verification;

namespace SoundShapesServer.Endpoints.Game;

public class AuthenticationEndpoints : EndpointGroup
{
    [GameEndpoint("identity/login/token/psn.post", HttpMethods.Post)]
    [HttpEndpoint("/identity/login/token/psn", HttpMethods.Post)]
    [RateLimitSettings(300, 10, 300, "authentication")]
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

        if (!UserHelper.IsUsernameLegal(ticket.Username)) 
            return HttpStatusCode.BadRequest;

        GameUser? user = database.GetUserWithUsername(ticket.Username, true);
        if (user == null && !config.AccountCreation)
        {
            return new Response(HttpStatusCode.Created);
        }
        user ??= database.CreateUser(ticket.Username);

        PlatformType platformType = PlatformHelper.GetPlatformType(ticket);
        GameIp? gameIp = context.GetGameIp(database, user);
        bool genuineTicket = VerifyTicket(context, (MemoryStream)body, ticket);

        TokenType? tokenType;
        TokenAuthenticationType authenticationType;

        if (config.RequireAuthentication)
        {
            if (user.AllowIpAuthentication)
                gameIp ??= database.CreateGameIp(user, context.GetIpAddress());
            
            if (!user.HasFinishedRegistration)
            {
                tokenType = TokenType.GameUnAuthorized;
                authenticationType = TokenAuthenticationType.Ip;
            }
            else if (genuineTicket && platformType is PlatformType.Ps3 or PlatformType.PsVita && user.AllowPsnAuthentication)
            {
                tokenType = TokenType.GameAccess;
                authenticationType = TokenAuthenticationType.Psn;
            }
            else if (genuineTicket && platformType is PlatformType.Rpcs3 && user.AllowRpcnAuthentication)
            {
                tokenType = TokenType.GameAccess;
                authenticationType = TokenAuthenticationType.Rpcn;
            }
            else if (gameIp?.Authorized == true)
            {
                tokenType = TokenType.GameAccess;
                authenticationType = TokenAuthenticationType.Ip;
                if (gameIp.OneTimeUse) 
                    database.UseOneTimeIpAddress(gameIp);
            }
            else
            {
                tokenType = TokenType.GameUnAuthorized;
                authenticationType = TokenAuthenticationType.None;
            }
        }
        else
        {
            tokenType = TokenType.GameAccess;
            authenticationType = TokenAuthenticationType.None;
        }
        
        if (user.Deleted || user.PermissionsType == PermissionsType.Banned)
            tokenType = TokenType.GameUnAuthorized;
        
        GameToken token = database.CreateToken(user, (TokenType)tokenType, authenticationType, Globals.FourHoursInSeconds, platformType, genuineTicket);
        AuthenticationResponse responseWrapper = new (token);

        context.Logger.LogInfo(BunkumCategory.Authentication, $"{token.User.Username} has logged in. TokenType:" + Enum.GetName(token.TokenType));

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={token.Id};Version=1;Path=/");
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
    public string? Eula(RequestContext context, GameServerConfig config, BunkumConfig bunkumConfig, GameDatabaseContext database, string platform, string publisher, string language, GameToken? token, GameUser? user)
    {
        if (token?.TokenType == TokenType.GameAccess)
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
        
        if (user.PermissionsType == PermissionsType.Banned)
        {
            IQueryable<Punishment> bans = user.Punishments.ActiveBans();
            Punishment longestBan = bans.Last();
            
            return "You are banned.\n" +
                   "Expires at " + longestBan.ExpiryDate.Date + ".\n" + 
                   "Reason: \"" + longestBan.Reason + "\"" + eulaEnd;
        }
        if (user.HasFinishedRegistration == false)
        {
            GameToken registerToken = database.CreateToken(user, TokenType.AccountRegistration, TokenAuthenticationType.PreExistingToken, Globals.TenMinutesInSeconds);
            return $"Your account is not registered.\n \n" +
                   $"To proceed, you will have to register an account at {bunkumConfig.ExternalUrl}/register\n" +
                   $"Your registration code is: {registerToken.Id}" + eulaEnd;
        }
        
        string unAuthorizedBase = $"Your token has not been authenticated.\n \n" +
                             $"To proceed, you will have to log into your account at {bunkumConfig.ExternalUrl}/authentication " +
                             $"and perform one of the following actions:\n";
                
        List<string> authorizationMethods = new ();

        if (token!.GenuineNpTicket == true)
        {
            switch (token.PlatformType)
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