using SoundShapesServer.Database;
using SoundShapesServer.Types.PlayerActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivitiesWrapper
{
    public ApiPlayerActivitiesWrapper(GameDatabaseContext database, IEnumerable<GameEvent> events, int totalEvents)
    {
        Activities = events.Select(e=> new ApiPlayerActivityResponse(database, e)).ToArray();
        Count = totalEvents;
    }

    public ApiPlayerActivityResponse[] Activities { get; set; }
    public int Count { get; set; }
}