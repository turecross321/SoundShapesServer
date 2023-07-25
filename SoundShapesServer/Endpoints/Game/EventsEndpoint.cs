using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
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
    
    [GameEndpoint("~identity:{id}/~stream:news.page")]
    [GameEndpoint("~index:activity.page")]
    public ListResponse<EventResponse> GetEvents(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        EventFilters filters = EventHelper.GetEventFilters(context, database);
        filters.EventTypes ??= _gameEventTypes;
        EventOrderType order = EventHelper.GetEventOrder(context);
        
        (GameEvent[] events, int totalEvents) = database.GetEvents(order, descending, filters, from, count);
        return new ListResponse<EventResponse>(events.Select(e => new EventResponse(e)), totalEvents, from, count);
    }
}