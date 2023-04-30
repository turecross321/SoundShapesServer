using Realms;
using SoundShapesServer.Requests.Game;

namespace SoundShapesServer.Types.Levels;

public class LeaderboardEntry : RealmObject
{
    public LeaderboardEntry(string id, GameUser user, string levelId, LeaderboardSubmissionRequest request)
    {
        Id = id;
        User = user;
        LevelId = levelId;
        Score = request.Score;
        PlayTime = request.PlayTime;
        Deaths = request.Deaths;
        Golded = request.Golded;
        Tokens = request.Tokens;
        Completed = request.Completed;
        Date = DateTimeOffset.UtcNow;
    }

    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public LeaderboardEntry() { }
    #pragma warning restore CS8618

    public string Id { get; set; }
    public GameUser User { get; init; }
    public string LevelId { get; }
    public long Score { get; }
    public long PlayTime { get; }
    public int Deaths { get; }
    public int Golded { get; set; }
    public int Tokens { get; }
    public bool Completed { get; }
    public DateTimeOffset Date { get; }
}