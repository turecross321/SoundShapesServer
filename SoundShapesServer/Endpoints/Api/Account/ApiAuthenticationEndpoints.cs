using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.RateLimit;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Account;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    [ApiEndpoint("account/logIn", HttpMethods.Post), Authentication(false)]
    [RateLimitSettings(300, 10, 300, "authentication")]
    [DocSummary("Returns an access token used to access endpoints that require authentication.")]
    [DocError(typeof(ApiForbiddenError), ApiForbiddenError.EmailOrPasswordIsWrongWhen)]
    public ApiResponse<ApiLoginResponse> Login(RequestContext context, GameDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) 
            return ApiForbiddenError.EmailOrPasswordIsWrong;

        if (!database.ValidatePassword(user, body.PasswordSha512))
            return ApiForbiddenError.EmailOrPasswordIsWrong;

        GameToken refreshToken = database.CreateToken(user, TokenType.ApiRefresh, Globals.OneMonthInSeconds);
        GameToken accessToken = database.CreateToken(user, TokenType.ApiAccess, refreshToken:refreshToken);

        return new ApiLoginResponse(user, accessToken, refreshToken);
    }
    
    [ApiEndpoint("account/refreshToken", HttpMethods.Post), Authentication(false)]
    [RateLimitSettings(300, 10, 300, "authentication")]
    [DocSummary("Generates a new access token with a refresh token serving as authentication.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.RefreshTokenDoesNotExistWhen)]
    public ApiResponse<ApiLoginResponse> RefreshToken(RequestContext context, GameDatabaseContext database, ApiRefreshTokenRequest body)
    {
        GameToken? refreshToken = database.GetTokenWithId(body.RefreshTokenId, TokenType.ApiRefresh);
        if (refreshToken == null || DateTimeOffset.UtcNow > refreshToken.ExpiryDate)
            return ApiNotFoundError.RefreshTokenDoesNotExist;
        
        database.RefreshToken(refreshToken, Globals.OneMonthInSeconds);
        GameToken accessToken = database.CreateToken(refreshToken.User, TokenType.ApiAccess, refreshToken:refreshToken);
        
        return new ApiLoginResponse(accessToken.User, accessToken, refreshToken);
    }
    
    [ApiEndpoint("account/logOut", HttpMethods.Post)]
    [DocSummary("Revokes the access token (and the associated refresh token if it exists) used to make this request.")]
    public ApiOkResponse Logout(RequestContext context, GameDatabaseContext database, GameToken token)
    {
        if (token.RefreshToken != null)
        {
            GameToken refreshToken = token.RefreshToken;
            database.RemoveToken(refreshToken.RefreshableTokens);
            database.RemoveToken(refreshToken);
        }
        else
        {
            database.RemoveToken(token);
        }
        
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
    public ApiOkResponse AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthenticateIpRequest body, GameUser user)
    {
        GameIp? gameIp = database.GetIpWithAddress(user, body.IpAddress);
        gameIp ??= database.CreateGameIp(user, body.IpAddress);
        database.AuthorizeIpAddress(gameIp, body.OneTimeUse);

        return new ApiOkResponse();
    }
    [ApiEndpoint("gameAuth/ip/address/{address}", HttpMethods.Delete)]
    [DocSummary("Deletes specified IP address.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.IpDoesNotExistWhen)]
    public ApiOkResponse RemoveIpAddress(RequestContext context, GameDatabaseContext database, string address, GameUser user)
    {
        GameIp? gameIp = database.GetIpWithAddress(user, address);
        if (gameIp == null)
            return ApiNotFoundError.IpDoesNotExist;

        database.RemoveIpAddress(gameIp);
        return new ApiOkResponse();
    }

    [ApiEndpoint("gameAuth/ip")]
    [DocUsesPageData]
    [DocSummary("List IP addresses that have attempted to connect with the user's username.")]
    [DocQueryParam("authorized", "Filters authorized/unauthorized IP addresses from result.")]
    public ApiListResponse<ApiIpResponse> GetAddresses(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = context.GetPageData();
        
        bool? authorized = null;
        if (bool.TryParse(context.QueryString["authorized"], out bool authorizedTemp)) authorized = authorizedTemp;
        
        (GameIp[] addresses, int totalAddresses) =
            database.GetPaginatedIps(user, authorized, from, count);

        return new ApiListResponse<ApiIpResponse>(addresses.Select(a=>new ApiIpResponse(a)), totalAddresses);
    }
}