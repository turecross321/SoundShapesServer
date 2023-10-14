using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Configuration;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses;

namespace SoundShapesServer.Endpoints.Api;

public class ApiEulaEndpoints : EndpointGroup
{
    [ApiEndpoint("eula"), Authentication(false)]
    public ApiResponse<ApiEulaResponse> GetEula(RequestContext context, GameServerConfig config)
    {
        return new ApiEulaResponse(config.EulaText);
    }
}