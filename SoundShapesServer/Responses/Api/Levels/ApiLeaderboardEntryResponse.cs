using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryResponse
{
    public ApiLeaderboardEntryResponse(LeaderboardEntry entry, int position)
    {
        Position = position;

        UserId = entry.User.Id;
        Username = entry.User.Username;

        Score = entry.Score;
        PlayTime = entry.PlayTime;
        Tokens = entry.Tokens;
        Deaths = entry.Deaths;
        Date = entry.Date;
    }

    public int Position { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    
    public long Score { get; set; }
    public long PlayTime { get; set; }
    public int Tokens { get; set; }
    public int Deaths { get; set; }
    
    public DateTimeOffset Date { get; set; }
}