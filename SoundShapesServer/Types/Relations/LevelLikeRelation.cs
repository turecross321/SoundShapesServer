using Realms;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Type.Relations;

public class LevelLikeRelation : RealmObject
{
    public GameUser liker { get; set; }
    public GameLevel level { get; set; }
}