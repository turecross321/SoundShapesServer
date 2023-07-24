using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Events;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventsEndpoint : EndpointGroup
{
    [ApiEndpoint("events"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists events.")]
    public ApiEventsWrapper GetActivities(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        EventFilters filters = EventHelper.GetEventFilters(context, database);
        EventOrderType orderType = EventHelper.GetEventOrder(context);
        
        (GameEvent[] events, int totalEvents) = database.GetEvents(orderType, descending, filters, from, count);
        
        return new ApiEventsWrapper(database, events, totalEvents);
    }
}