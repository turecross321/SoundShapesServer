using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;

namespace SoundShapesServer.Endpoints;

public class TranslationEndpoints : EndpointGroup
{
    [Endpoint("/otg/{platform}/{publisher}/{language}/~translation.get")]
    public Response GetTranslation(RequestContext context, string platform, string publisher, string language)
    {
        // This is for Album Translations
        return new Response(HttpStatusCode.OK);
    }
}
