using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Responses.Game.RecentActivity;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class PlayerActivityEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{id}/~stream:news.page", ContentType.Json)]
    public PlayerNewsResponse PlayerNews(RequestContext context)
    {
        return new PlayerNewsResponse() { Items = new string[0] };
    }
}