using Realms;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class GameEvent : RealmObject
{
    public GameEvent(GameUser actor, EventType eventType, GameUser? user, GameLevel? level, LeaderboardEntry? leaderboardEntry)
    {
        Id = Guid.NewGuid().ToString();
        Actor = actor;

        ContentUser = user;
        ContentLevel = level;
        ContentLeaderboardEntry = leaderboardEntry;
        
        EventType = eventType;
        CreationDate = DateTimeOffset.UtcNow;
    }
    
    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public GameEvent() {}
#pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; set; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with EventType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _EventType { get; set; }
    public EventType EventType
    {
        get => (EventType)_EventType;
        set => _EventType = (int)value;
    }
    
    public GameUser Actor { get; init; }
    public GameUser? ContentUser { get; init; }
    public GameLevel? ContentLevel { get; set; }
    
    public LeaderboardEntry? ContentLeaderboardEntry { get; set; }
    public DateTimeOffset CreationDate { get; set; }
}