using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Api.Account;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    [ApiEndpoint("account/logIn", HttpMethods.Post), Authentication(false)]
    [DocError(typeof(ApiForbiddenError), ApiForbiddenError.EmailOrPasswordIsWrongWhen)]
    public ApiResponse<ApiSessionResponse> Login(RequestContext context, GameDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) 
            return ApiForbiddenError.EmailOrPasswordIsWrong;

        if (!database.ValidatePassword(user, body.PasswordSha512))
            return ApiForbiddenError.EmailOrPasswordIsWrong;

        IQueryable<Punishment> activeBans = GetActiveUserBans(user);
        SessionType sessionType = activeBans.Any() ? SessionType.Banned : SessionType.Api;
        GameSession session = database.CreateSession(user, sessionType, PlatformType.Api);
        
        return new ApiSessionResponse(session, activeBans);
    }

    [ApiEndpoint("account/logOut", HttpMethods.Post)]
    [DocSummary("Revokes the session used to make this request.")]
    public ApiOkResponse Logout(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        database.RemoveSession(session);
        return new ApiOkResponse();
    }
    
    // Game Authentication
    
    [ApiEndpoint("gameAuth/settings", HttpMethods.Post)]
    [DocSummary("Sets user's game authentication settings.")]
    public ApiOkResponse SetGameAuthenticationSettings(RequestContext context, GameDatabaseContext database, GameUser user, ApiSetGameAuthenticationSettingsRequest body)
    {
        database.SetUserGameAuthenticationSettings(user, body);
        return new ApiOkResponse();
    }

    [ApiEndpoint("gameAuth/settings")]
    [DocSummary("Lists user's game authentication settings.")]
    public ApiResponse<ApiGameAuthenticationSettingsResponse> GetGameAuthenticationSettings(RequestContext context, GameUser user) 
        => new ApiGameAuthenticationSettingsResponse(user);
    
    [ApiEndpoint("gameAuth/ip/authorize", HttpMethods.Post)]
    [DocSummary("Authorizes specified IP address.")]
    [DocError(typeof(ApiConflictError), ApiConflictError.AlreadyAuthenticatedIpWhen)]
    public ApiOkResponse AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthenticateIpRequest body, GameUser user)
    {
        GameIp gameIp = database.GetIpFromAddress(user, body.IpAddress);

        if (!database.AuthorizeIpAddress(gameIp, body.OneTimeUse))
            return ApiConflictError.AlreadyAuthenticatedIp;

        return new ApiOkResponse();
    }
    [ApiEndpoint("gameAuth/ip/address/{address}", HttpMethods.Delete)]
    [DocSummary("Deletes specified IP address.")]
    public ApiOkResponse UnAuthorizeIpAddress(RequestContext context, GameDatabaseContext database, string address, GameUser user)
    {
        GameIp gameIp = database.GetIpFromAddress(user, address);

        database.RemoveIpAddress(gameIp);
        return new ApiOkResponse();
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