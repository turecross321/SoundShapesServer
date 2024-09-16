using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Common.Types.Config;
using SoundShapesServer.Common.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Common.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class EulaEndpoints : EndpointGroup
{
    [DocSummary("Retrieves the server's terms and conditions.")]
    [DocResponseBody(typeof(ApiEulaResponse))]
    [Authentication(false)]
    [ApiEndpoint("eula")]
    public ApiResponse<ApiEulaResponse> GetEula(RequestContext context, ServerConfig config)
    {
        return new ApiEulaResponse
        {
            CustomText = config.EulaText,
            License = Licenses.AGPLNotice,
            RepositoryUrl = config.RepositoryUrl
        };

    }
}