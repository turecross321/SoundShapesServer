using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Api.Account;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    private readonly Response _invalidCredentialsResponse = new (ForbiddenError.EmailOrPasswordIsWrongWhen, ContentType.Plaintext, HttpStatusCode.Forbidden);
    
    [ApiEndpoint("account/logIn", Method.Post), Authentication(false)]
    [DocError(typeof(ForbiddenError), ForbiddenError.EmailOrPasswordIsWrongWhen)]
    public Response Login(RequestContext context, GameDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return _invalidCredentialsResponse;

        if (!database.ValidatePassword(user, body.PasswordSha512)) return _invalidCredentialsResponse;

        IQueryable<Punishment> activeBans = GetActiveUserBans(user);
        SessionType sessionType = activeBans.Any() ? SessionType.Banned : SessionType.Api;
        GameSession session = database.CreateSession(user, sessionType, PlatformType.Api);
        
        return new Response(new ApiSessionResponse(session, activeBans), ContentType.Json);
    }

    [ApiEndpoint("account/logOut", Method.Post)]
    [DocSummary("Revokes the session used to make this request.")]
    public Response Logout(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        database.RemoveSession(session);
        return HttpStatusCode.NoContent;
    }
    
    // Game Authentication
    
    [ApiEndpoint("gameAuth/settings", Method.Post)]
    [DocSummary("Sets user's game authentication settings.")]
    public Response SetGameAuthenticationSettings(RequestContext context, GameDatabaseContext database, GameUser user, ApiSetGameAuthenticationSettingsRequest body)
    {
        database.SetUserGameAuthenticationSettings(user, body);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("gameAuth/settings")]
    [DocSummary("Lists user's game authentication settings.")]
    public ApiGameAuthenticationSettingsResponse GetGameAuthenticationSettings(RequestContext context, GameUser user) => new (user);
    
    [ApiEndpoint("gameAuth/ip/authorize", Method.Post)]
    [DocSummary("Authorizes specified IP address.")]
    [DocError(typeof(ConflictError), ConflictError.AlreadyAuthenticatedIpWhen)]
    public Response AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthenticateIpRequest body, GameUser user)
    {
        GameIp gameIp = database.GetIpFromAddress(user, body.IpAddress);

        if (!database.AuthorizeIpAddress(gameIp, body.OneTimeUse))
            return new Response(ConflictError.AlreadyAuthenticatedIpWhen, ContentType.Plaintext, HttpStatusCode.Conflict);
        
        return HttpStatusCode.Created;
    }
    [ApiEndpoint("gameAuth/ip/address/{address}", Method.Delete)]
    [DocSummary("Deletes specified IP address.")]
    public Response UnAuthorizeIpAddress(RequestContext context, GameDatabaseContext database, string address, GameUser user)
    {
        GameIp gameIp = database.GetIpFromAddress(user, address);

        database.RemoveIpAddress(gameIp);
        return HttpStatusCode.NoContent;
    }

    [ApiEndpoint("gameAuth/ip")]
    [DocUsesPageData]
    [DocSummary("List IP addresses that have attempted to connect with the user's username.")]
    [DocQueryParam("authorized", "Filters authorized/unauthorized IP addresses from result.")]
    public ApiListResponse<ApiIpResponse> GetAddresses(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);
        
        bool? authorized = null;
        if (bool.TryParse(context.QueryString["authorized"], out bool authorizedTemp)) authorized = authorizedTemp;
        
        (GameIp[] addresses, int totalAddresses) =
            database.GetPaginatedIps(user, authorized, from, count);

        return new ApiListResponse<ApiIpResponse>(addresses.Select(a=>new ApiIpResponse(a)), totalAddresses);
    }
}