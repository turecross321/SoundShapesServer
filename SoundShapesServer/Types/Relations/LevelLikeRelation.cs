using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelLikeRelation : RealmObject
{
    public LevelLikeRelation(DateTimeOffset date, GameUser user, GameLevel level)
    {
        Date = date;
        User = user;
        Level = level;
    }
    
    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public LevelLikeRelation() {}
#pragma warning restore CS8618

    public DateTimeOffset Date { get; set; }
    public GameUser User { get; set; }
    public GameLevel Level { get; init; }
}