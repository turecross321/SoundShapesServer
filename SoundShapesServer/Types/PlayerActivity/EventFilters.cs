using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.PlayerActivity;

public class EventFilters
{
    public GameUser[]? Actors { get; set; }
    public GameUser? OnUser { get; set; }
    public GameLevel? OnLevel { get; set; }
    public EventType[]? EventTypes { get; set; }

    public EventFilters(GameUser[]? actors = null, GameUser? onUser = null, GameLevel? onLevel = null, EventType[]? eventTypes = null)
    {
        Actors = actors;
        OnUser = onUser;
        OnLevel = onLevel;
        EventTypes = eventTypes;
    }
}