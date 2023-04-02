using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;

namespace SoundShapesServer.Endpoints.RecentActivity;

public class FeaturedEndpoints : EndpointGroup
{
    [Endpoint("/otg/global/featured/{language}/~metadata.get", ContentType.Json)]
    [Authentication(true)]
    public Response Featured(RequestContext context, string language)
    {
        return new Response(HttpStatusCode.OK);
    }
}