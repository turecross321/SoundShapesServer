using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.RecentActivity;

public class GameEvent : RealmObject
{
    public GameEvent(GameUser actor, GameUser? user, GameLevel? level, LeaderboardEntry? leaderboardEntry, EventType eventType)
    {
        Id = Guid.NewGuid().ToString();
        Actor = actor;

        ContentUser = user;
        ContentLevel = level;
        ContentLeaderboardEntry = leaderboardEntry;
        
        EventType = (int)eventType;
        Date = DateTimeOffset.UtcNow;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameEvent() {}
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; set; }
    public int EventType { get; set; }
    public GameUser Actor { get; init; }
    
    // This is not very clean, but I can't think of any other way to do it.
    public GameUser? ContentUser { get; init; }
    public GameLevel? ContentLevel { get; init; }
    
    public LeaderboardEntry? ContentLeaderboardEntry { get; set; }
    public DateTimeOffset Date { get; set; }
}