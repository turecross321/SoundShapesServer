using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivitiesWrapper
{
    public ApiPlayerActivitiesWrapper(GameEvent[] events, int totalEvents)
    {
        Activities = events.Select(e=> new ApiPlayerActivityResponse(e)).ToArray();
        Count = totalEvents;
    }

    public ApiPlayerActivityResponse[] Activities { get; set; }
    public int Count { get; set; }
}