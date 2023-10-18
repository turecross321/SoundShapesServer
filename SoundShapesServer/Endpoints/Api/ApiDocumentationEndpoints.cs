using AttribDoc;
using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Documentation;
using SoundShapesServer.Services;

namespace SoundShapesServer.Endpoints.Api;

public class ApiDocumentationEndpoints : EndpointGroup
{
    [ApiEndpoint("documentation"), Authentication(false)]
    [DocSummary("Retrieves a JSON object containing documentation about the API.")]
    [ClientCacheResponse(Globals.OneHourInSeconds)]
    public ApiListResponse<ApiRouteResponse> GetDocumentation(RequestContext context, DocumentationService documentation)
    {
        return new ApiListResponse<ApiRouteResponse>(ApiRouteResponse.FromRouteList(documentation.Documentation), documentation.Documentation.Count());
    }
}