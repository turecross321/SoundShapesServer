using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelQueueRelation: RealmObject
{
    public LevelQueueRelation(DateTimeOffset date, GameUser user, GameLevel level)
    {
        Date = date;
        User = user;
        Level = level;
    }
    
    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public LevelQueueRelation() {}
#pragma warning restore CS8618

    public DateTimeOffset Date { get; set; }
    public GameUser User { get; init; }
    public GameLevel Level { get; init; }
}