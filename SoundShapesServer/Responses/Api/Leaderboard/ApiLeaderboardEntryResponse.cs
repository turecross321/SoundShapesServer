using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Api.Leaderboard;

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
        Notes = entry.Notes;
        Deaths = entry.Deaths;
        Completed = entry.Completed;
        CreationDate = entry.CreationDate.ToUnixTimeSeconds();
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
    public int Notes { get; set; }
    public int Deaths { get; set; }
    public bool Completed { get; set; }
    public long CreationDate { get; set; }
}