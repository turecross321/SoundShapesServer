using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Responses;

namespace SoundShapesServer.Endpoints.RecentActivity;

public class PlayerActivityEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~stream:news.page", ContentType.Json)]
    public PlayerNewsResponse PlayerNews(RequestContext context)
    {
        return new PlayerNewsResponse() { items = new string[0] };
    }
}