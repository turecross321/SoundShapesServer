using SoundShapesServer.Types.RecentActivity;
using static SoundShapesServer.Helpers.PaginationHelper;
using static SoundShapesServer.Helpers.RecentActivityHelper;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivitiesWrapper
{
    public ApiPlayerActivitiesWrapper(IQueryable<GameEvent> events, int from, int count, EventOrderType orderType, bool descending)
    {
        IQueryable<GameEvent> orderedEvents = OrderEvents(events, orderType, descending);
        GameEvent[] paginatedEvents = PaginateEvents(orderedEvents, from, count);
        
        Activities = paginatedEvents.Select(e=> new ApiPlayerActivityResponse(e)).ToArray();
        Count = orderedEvents.Count();
    }

    public ApiPlayerActivityResponse[] Activities { get; set; }
    public int Count { get; set; }
}