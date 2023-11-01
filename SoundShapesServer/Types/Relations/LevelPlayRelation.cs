using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelPlayRelation : RealmObject, ILevelRelation
{
    public GameUser User { get; set; }
    public GameLevel Level { get; init; }
    public DateTimeOffset Date { get; init; }
}