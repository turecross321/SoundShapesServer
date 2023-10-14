using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Responses.Api.Framework.Documentation;
using SoundShapesServer.Services;

namespace SoundShapesServer.Endpoints.Api;

public class ApiDocumentationEndpoints : EndpointGroup
{
    [ApiEndpoint("documentation"), Authentication(false)]
    [DocSummary("Retrieves a JSON object containing documentation about the API.")]
    [ClientCacheResponse(Globals.OneHourInSeconds)]
    public IEnumerable<ApiRouteResponse> GetDocumentation(RequestContext context, DocumentationService documentation)
    {
        return documentation.Documentation;
    }
}