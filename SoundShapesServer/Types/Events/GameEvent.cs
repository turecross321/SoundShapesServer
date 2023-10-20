using Realms;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class GameEvent : RealmObject
{
    [PrimaryKey] [Required] public string Id { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with EventType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _EventType { get; set; }
    public EventType EventType
    {
        get => (EventType)_EventType;
        set => _EventType = (int)value;
    }

    public GameUser Actor { get; init; } = null!;
    public GameUser? ContentUser { get; init; }
    public GameLevel? ContentLevel { get; set; }
    
    public LeaderboardEntry? ContentLeaderboardEntry { get; init; }
    public DateTimeOffset CreationDate { get; set; }
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; init; } = (int)PlatformType.Unknown;
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        init => _PlatformType = (int)value;
    }
}