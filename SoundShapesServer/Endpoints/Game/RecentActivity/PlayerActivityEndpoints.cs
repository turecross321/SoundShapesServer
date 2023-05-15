using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class PlayerActivityEndpoints : EndpointGroup
{
    private readonly EventType[] _gameEventTypes =
    {
        EventType.Publish,
        EventType.Like,
        EventType.Follow
    };
    
    [GameEndpoint("~identity:{id}/~stream:news.page", ContentType.Json)]
    public ActivitiesWrapper GetPlayerActivity(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        EventFilters filters = new (eventTypes:_gameEventTypes);
        
        (GameEvent[] events, int totalEvents) = database.GetEvents(EventOrderType.Date, true, filters, from, count);
        return new ActivitiesWrapper(events, totalEvents, from, count);
    }
}