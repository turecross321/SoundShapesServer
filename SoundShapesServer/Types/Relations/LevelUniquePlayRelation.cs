using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelUniquePlayRelation : RealmObject
{
    public GameUser User { get; init; }
    public GameLevel Level { get; init; }
    public DateTimeOffset Date { get; set; }
}