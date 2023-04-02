using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class LevelFavoriteRelation : RealmObject
{
    public GameUser user { get; set; }
    public GameLevel GameLevel { get; set; }
}