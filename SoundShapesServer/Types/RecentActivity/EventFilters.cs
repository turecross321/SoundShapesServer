using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.RecentActivity;

public class EventFilters
{
    public GameUser[]? Actors;
    public GameUser? OnUser;
    public GameLevel? OnLevel;
    public EventType[]? EventTypes;

    public EventFilters(GameUser[]? actors = null, GameUser? onUser = null, GameLevel? onLevel = null, EventType[]? eventTypes = null)
    {
        Actors = actors;
        OnUser = onUser;
        OnLevel = onLevel;
        EventTypes = eventTypes;
    }
}