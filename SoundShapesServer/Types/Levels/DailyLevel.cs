using MongoDB.Bson;
using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public string Id { get; set; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}