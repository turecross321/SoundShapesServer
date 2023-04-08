using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class Report : RealmObject
{
    public ObjectId Id = ObjectId.GenerateNewId();
    public GameUser reporter { get; set; }
    public GameLevel level { get; set; }
    public int reportReasonId { get; set; }
}