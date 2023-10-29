using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelPlayRelation : RealmObject
{
    public GameUser User { get; set; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}