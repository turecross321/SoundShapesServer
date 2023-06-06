using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Events;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventsEndpoint : EndpointGroup
{
    [ApiEndpoint("events")]
    [Authentication(false)]
    public ApiEventsWrapper GetActivities(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        
        EventFilters filters = EventHelper.GetEventFilters(context, database);
        EventOrderType orderType = EventHelper.GetEventOrder(context);
        
        (GameEvent[] events, int totalEvents) = database.GetEvents(orderType, descending, filters, from, count);
        
        return new ApiEventsWrapper(database, events, totalEvents);
    }
}