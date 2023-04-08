using MongoDB.Bson;
using Realms;

namespace SoundShapesServer.Types;

public class LeaderboardEntry : RealmObject
{
    public ObjectId id = ObjectId.GenerateNewId();
    public GameUser user { get; set; }
    public string levelId { get; set; }
    public long score { get; set; }
    public int playTime { get; set; }
    public int deaths { get; set; }
    public int golded { get; set; }
    public int tokenCount { get; set; }
    public bool completed { get; set; }
    public DateTimeOffset date { get; set; }
}