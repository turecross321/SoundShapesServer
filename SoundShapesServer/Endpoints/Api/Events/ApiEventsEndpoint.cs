using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventsEndpoint : EndpointGroup
{
    [ApiEndpoint("events"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists events.")]
    public ApiListResponse<ApiEventResponse> GetActivities(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        EventFilters filters = EventHelper.GetEventFilters(context, database);
        EventOrderType orderType = EventHelper.GetEventOrder(context);
        
        (GameEvent[] events, int totalEvents) = database.GetPaginatedEvents(orderType, descending, filters, from, count);
        
        return new ApiListResponse<ApiEventResponse>(events.Select(e=>new ApiEventResponse(database, e)), totalEvents);
    }
}