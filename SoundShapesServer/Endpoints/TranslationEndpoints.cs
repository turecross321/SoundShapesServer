using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;

namespace SoundShapesServer.Endpoints;

public class TranslationEndpoints : EndpointGroup
{
    [Endpoint("/otg/ps3/{publisher}/{language}/~translation.get")]
    public Response GetTranslation(RequestContext context, string publisher, string language)
    {
        // Not entirely sure what this is supposed to return
        return new Response(HttpStatusCode.OK);
    }
}
