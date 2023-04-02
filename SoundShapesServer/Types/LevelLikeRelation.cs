using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class LevelLikeRelation : RealmObject
{
    public GameUser liker { get; set; }
    public GameLevel level { get; set; }
}