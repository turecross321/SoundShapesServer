using MongoDB.Bson;
using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public ObjectId Id = ObjectId.GenerateNewId(); 
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}