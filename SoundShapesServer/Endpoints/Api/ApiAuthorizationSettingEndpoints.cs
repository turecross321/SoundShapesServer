using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAuthorizationSettingEndpoints : EndpointGroup
{
    [DocSummary("Retrieve your game authorization settings.")]
    [DocResponseBody(typeof(ApiAuthorizationSettingsResponse))]
    [ApiEndpoint("gameAuth")]
    public ApiResponse<ApiAuthorizationSettingsResponse> GetAuthorizationSettings(RequestContext context, DbUser user)
    {
        return new ApiAuthorizationSettingsResponse
        {
            RpcnAuthorization = user.RpcnAuthorization,
            PsnAuthorization = user.PsnAuthorization,
            IpAuthorization = user.IpAuthorization
        };
    }
    
    [DocSummary("Set what forms of game authorization that shall be used.")]
    [DocRequestBody(typeof(ApiAuthorizationSettingsRequest))]
    [DocResponseBody(typeof(ApiAuthorizationSettingsResponse))]
    [ApiEndpoint("gameAuth", HttpMethods.Put)]
    public ApiResponse<ApiAuthorizationSettingsResponse> SetAuthorizationSettings(RequestContext context, GameDatabaseContext database, 
        DbUser user, ApiAuthorizationSettingsRequest body)
    {
        user = database.SetUserAuthorizationSettings(user, body);
        return new ApiAuthorizationSettingsResponse
        {
            RpcnAuthorization = user.RpcnAuthorization,
            PsnAuthorization = user.PsnAuthorization,
            IpAuthorization = user.IpAuthorization
        };
    }

    [DocError(typeof(ApiPreconditionFailedError), ApiPreconditionFailedError.IpAuthorizationDisabledWhen)]
    [ApiEndpoint("gameAuth/ip")]
    public ApiListResponse<ApiIpResponse> GetIps(RequestContext context, GameDatabaseContext database, DbUser user)
    {
        if (!user.IpAuthorization)
        {
            return ApiPreconditionFailedError.IpAuthorizationDisabled;
        }

        IQueryable<DbIp> ips = database.GetIpsWithUser(user);
        PaginatedDbList<DbIp, int> paginated = ips.PaginateWithIntId(context.GetPageData());

        return ApiListResponse<ApiIpResponse>.FromPaginatedList<DbIp, int, ApiIpResponse>(paginated);
    }
}