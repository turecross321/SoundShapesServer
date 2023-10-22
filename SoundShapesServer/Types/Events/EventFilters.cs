using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class EventFilters
{
    public List<GameUser>? Actors { get; set; }
    public GameUser? OnUser { get; set; }
    public GameLevel? OnLevel { get; set; }
    public List<EventType>? EventTypes { get; set; }
    public DateTimeOffset? CreatedBefore { get; init; }
    public DateTimeOffset? CreatedAfter { get; init; }
}