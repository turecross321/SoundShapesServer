using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId(); 
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public GameUser Author { get; set; }
}