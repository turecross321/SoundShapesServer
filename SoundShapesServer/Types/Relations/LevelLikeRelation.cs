using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Relations;

public class LevelLikeRelation : RealmObject
{
    public GameUser? Liker { get; init; }
    public GameLevel? Level { get; init; }
}