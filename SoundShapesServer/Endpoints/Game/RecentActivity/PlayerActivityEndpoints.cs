using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;
using SoundShapesServer.Types.RecentActivity;
using static SoundShapesServer.Helpers.RecentActivityHelper;

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
    public ActivitiesWrapper GetPlayerActivity(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        IQueryable<GameEvent> events = database.GetEvents();
        IQueryable<GameEvent> filteredEvents = FilterEvents(events, null, null, null, _gameEventTypes);
        IQueryable<GameEvent> orderedEvents = OrderEvents(filteredEvents, EventOrderType.Date, true);

        return new ActivitiesWrapper(orderedEvents, from, count);
    }
}