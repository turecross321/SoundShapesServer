using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelQueueRelation: RealmObject
{
    public DateTimeOffset Date { get; init; }
    public GameUser User { get; init; }
    public GameLevel Level { get; init; }
}