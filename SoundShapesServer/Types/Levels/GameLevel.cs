using Bunkum.HttpServer.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using Realms;
using SoundShapesServer.Database;

namespace SoundShapesServer.Types.Levels;

public class GameLevel : RealmObject
{
    public string id { get; set; }
    public GameUser author { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public LevelMetadata metadata { get; set; } = new LevelMetadata();
    public string visibility { get; set; }
    public int scp_np_language { get; set; }
    public long creationTime { get; set; }
}