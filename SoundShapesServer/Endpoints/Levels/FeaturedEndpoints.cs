using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Responses.RecentActivity;

namespace SoundShapesServer.Endpoints.Levels;

public class FeaturedEndpoints : EndpointGroup
{
    // TODO: MAKE THIS MODULAR
    [Endpoint("/otg/global/featured/{language}/~metadata:*.get", ContentType.Json)]
    public FeaturedResponse Featured(RequestContext context, string language)
    {
        return new FeaturedResponse()
        {
            queryType = "search",
            buttonLabel = "New Releases",
            query = "newest\u0026type\u003dlevel",
            panelDescription = "Check here daily for the latest cool levels! Always new stuff to check out!",
            imageUrl = "", // TODO: implement this
            panelTitle = "New Releases",
        };
    }
}