using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryResponse
{
    public ApiLeaderboardEntryResponse(LeaderboardEntry entry, int position)
    {
        Id = entry.Id;
        LevelId = entry.LevelId;
        Position = position;
        User = new ApiUserBriefResponse(entry.User);
        Score = entry.Score;
        PlayTime = entry.PlayTime;
        Tokens = entry.Tokens;
        Deaths = entry.Deaths;
        Completed = entry.Completed;
        Date = entry.Date;
    }
    
#pragma warning disable CS8618
    public ApiLeaderboardEntryResponse() {}
#pragma warning restore CS8618

    public string Id { get; set; }
    public string LevelId { get; set; } // Not a BriefLevelResponse because it should support campaign levels too
    public int Position { get; set; }
    public ApiUserBriefResponse User { get; set; }
    public long Score { get; set; }
    public long PlayTime { get; set; }
    public int Tokens { get; set; }
    public int Deaths { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset Date { get; set; }
}