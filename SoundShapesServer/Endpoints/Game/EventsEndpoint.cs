using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Events;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class EventsEndpoint : EndpointGroup
{
    private readonly EventType[] _gameEventTypes =
    {
        EventType.Publish,
        EventType.Like,
        EventType.Follow
    };
    
    [GameEndpoint("~identity:{id}/~stream:news.page", ContentType.Json)]
    [GameEndpoint("~index:activity.page", ContentType.Json)]
    public EventsWrapper GetEvents(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");

        EventFilters filters = EventHelper.GetEventFilters(context, database);
        EventOrderType order = EventHelper.GetEventOrder(context);
        
        (GameEvent[] events, int totalEvents) = database.GetEvents(order, descending, filters, from, count);
        return new EventsWrapper(events, totalEvents, from, count);
    }
}