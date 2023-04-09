using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class Report : RealmObject
{
    public ObjectId Id = ObjectId.GenerateNewId();
    public GameUser Reporter { get; set; }
    public GameLevel Level { get; set; }
    public int ReportReasonId { get; set; }
}