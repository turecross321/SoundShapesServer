using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Relations;

public class LevelLikeRelation : RealmObject
{
    public DateTimeOffset Date { get; set; }
    public GameUser Liker { get; init; } = new();
    public GameLevel Level { get; init; } = new();
}