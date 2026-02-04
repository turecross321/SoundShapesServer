using Bunkum.Core;
using Bunkum.Core.Endpoints;
using BunkumOpenApi.Attributes;
using SoundShapesServer.Attributes;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Documentation;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

[DocTag(ApiTags.InstanceInformation)]
public class ApiInstanceInformationEndpoints : EndpointGroup
{
    [DocSummary("Retrieves the server's terms and conditions.")]
    [DocApiResponse(typeof(ApiEulaResponse))]
    [Authentication(false)]
    [ApiEndpoint("eula")]
    public ApiResponse<ApiEulaResponse> GetEula(RequestContext context, ServerConfig config)
    {
        return new ApiEulaResponse
        {
            CustomText = config.EulaText,
            License = Licenses.AGPLNotice,
            RepositoryUrl = config.RepositoryUrl,
        };
    }
}