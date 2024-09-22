using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;
using SoundShapesServer.Types.Responses.Api.DataTypes.Documentation;

namespace SoundShapesServer.Endpoints.Api;

public class ApiDocumentationEndpoints : EndpointGroup
{
    [DocSummary("Retrieves documentation for all the API v1 endpoints.")]
    [DocResponseBody(typeof(ApiListResponse<ApiRouteResponse>))]
    [Authentication(false)]
    [ApiEndpoint("apiDocumentation")]
    public ApiListResponse<ApiRouteResponse> GetDocumentation(RequestContext context, ApiDocumentationService documentation)
    {
        return documentation.Routes.Select(ApiRouteResponse.FromRoute).ToList();
    }
}