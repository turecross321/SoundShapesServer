using Bunkum.Core;
using Bunkum.Core.Endpoints;
using BunkumOpenApi.Attributes;
using SoundShapesServer.Attributes;
using SoundShapesServer.Documentation;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

[DocTag(ApiTags.Users)]
public class ApiUserEndpoints : EndpointGroup
{
    [DocSummary("Retrieves the logged in user")]
    [DocApiResponse(typeof(ApiFullUserResponse))]
    [ApiEndpoint("users/me")]
    public ApiResponse<ApiFullUserResponse> GetSelf(RequestContext context, DbUser user)
    {
        return ApiFullUserResponse.FromDb(user);
    }
}