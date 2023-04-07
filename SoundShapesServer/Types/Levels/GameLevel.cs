using Bunkum.HttpServer.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using Realms;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types.Levels;

public class GameLevel : RealmObject
{
    public string id { get; set; }
    public GameUser author { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string visibility { get; set; }
    public int scp_np_language { get; set; }
    public DateTimeOffset created { get; set; }
    public DateTimeOffset modified { get; set; }
    public int plays { get; set; }
    public int deaths { get; set; }
    public IList<GameUser> uniquePlays { get; }
    public int completions { get; set; }
    public IList<GameUser> completionists { get; }
    [Backlink(nameof(LevelLikeRelation.level))]
    [JsonIgnore] public IQueryable<LevelLikeRelation> likes { get; }
}