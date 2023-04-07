using Realms;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Relations;

public class LevelLikeRelation : RealmObject
{
    public GameUser liker { get; set; }
    public GameLevel level { get; set; }
}