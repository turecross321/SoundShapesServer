using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class ApiUserEndpoints : EndpointGroup
{
    [DocSummary("Retrieves the logged in user")]
    [DocResponseBody(typeof(ApiFullUserResponse))]
    [ApiEndpoint("users/me")]
    public ApiFullUserResponse GetSelf(RequestContext context, DbUser user)
    {
        return ApiFullUserResponse.FromDb(user);
    }
}