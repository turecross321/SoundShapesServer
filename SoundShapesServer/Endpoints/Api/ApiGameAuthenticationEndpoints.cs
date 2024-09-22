using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class ApiGameAuthorizationEndpoints : EndpointGroup
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
}