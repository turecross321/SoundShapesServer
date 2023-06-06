using SoundShapesServer.Database;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Responses.Api.Events;

public class ApiEventsWrapper
{
    public ApiEventsWrapper(GameDatabaseContext database, IEnumerable<GameEvent> events, int totalEvents)
    {
        Events = events.Select(e=> new ApiEventResponse(database, e)).ToArray();
        Count = totalEvents;
    }

    public ApiEventResponse[] Events { get; set; }
    public int Count { get; set; }
}