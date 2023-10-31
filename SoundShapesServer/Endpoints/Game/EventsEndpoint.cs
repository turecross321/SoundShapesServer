using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.RequestContextExtensions;
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
        EventType.LevelPublish,
        EventType.LevelLike,
        EventType.UserFollow
    };
    
    [GameEndpoint("~identity:{id}/~stream:news.page")]
    [GameEndpoint("~index:activity.page")]
    public ListResponse<EventResponse> GetEvents(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        EventFilters filters = context.GetFilters<EventFilters>(database);
        filters.EventTypes ??= _gameEventTypes.ToIntArray();
        EventOrderType order = context.GetEventOrder();
        
        (GameEvent[] events, int totalEvents) = database.GetPaginatedEvents(order, descending, filters, from, count, user);
        return new ListResponse<EventResponse>(events.Select(e => new EventResponse(database, e)), totalEvents, from, count);
    }
}