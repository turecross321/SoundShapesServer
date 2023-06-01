using Realms;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardEntry : RealmObject
{
    public LeaderboardEntry(string id, GameUser user, string levelId, LeaderboardSubmissionRequest request)
    {
        Id = id;
        User = user;
        LevelId = levelId;
        Score = request.Score;
        PlayTime = Math.Max(request.PlayTime, 0);
        Deaths = Math.Max(request.Deaths, 0);
        Golded = request.Golded;
        Tokens = request.Tokens;
        Completed = request.Completed;
        Date = DateTimeOffset.UtcNow;
    }

    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public LeaderboardEntry() { }
#pragma warning restore CS8618

    [PrimaryKey]
    [Required] public string Id { get; set; }
    public GameUser User { get; init; }
    public string LevelId { get; set; }
    public long Score { get; set; }
    public long PlayTime { get; set; }
    public int Deaths { get; set; }
    public int Golded { get; set; }
    public int Tokens { get; set; } // TODO: RENAME TO TOKEN COUNT
    public bool Completed { get; set; }
    public DateTimeOffset Date { get; set; }
}