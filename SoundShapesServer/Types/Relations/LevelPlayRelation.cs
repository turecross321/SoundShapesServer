using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelPlayRelation : RealmObject
{
    public LevelPlayRelation(GameUser user, GameLevel level, DateTimeOffset date)
    {
        User = user;
        Level = level;
        Date = date;
    }

    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public LevelPlayRelation() {}
#pragma warning restore CS8618
    
    public GameUser User { get; set; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}