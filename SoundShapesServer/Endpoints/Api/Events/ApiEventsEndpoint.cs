using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Responses.Events;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventsEndpoint : EndpointGroup
{
    [ApiEndpoint("events")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<EventFilters>]
    [DocUsesOrder<UserOrderType>]
    [DocSummary("Lists events.")]
    public ApiEventsWrapperResponse GetEvents(RequestContext context, GameDatabaseContext database,
        GameUser? user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        EventFilters filters = context.GetFilters<EventFilters>(database);
        EventOrderType orderType = context.GetOrderType<EventOrderType>() ?? EventOrderType.CreationDate;

        PaginatedList<GameEvent>
            events = database.GetPaginatedEvents(orderType, descending, filters, from, count, user);
        return new ApiEventsWrapperResponse(database, events);
    }
}