using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Documentation;
using SoundShapesServer.Responses.Api.Documentation;
using SoundShapesServer.Services;

namespace SoundShapesServer.Endpoints.Api;

public class ApiDocumentationEndpoint : EndpointGroup
{
    [ApiEndpoint("documentation"), Authentication(false)]
    [DocSummary("Retrieves a JSON object containing documentation about the API.")]
    [ClientCacheResponse(Globals.OneHourInSeconds)]
    public IEnumerable<ApiRouteResponse> GetDocumentation(RequestContext context, DocumentationService service)
    {
        return service.Documentation;
    }
}