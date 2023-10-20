using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class LevelLikeRelation : RealmObject
{
    public DateTimeOffset Date { get; init; }
    public GameUser User { get; set; }
    public GameLevel Level { get; init; }
}