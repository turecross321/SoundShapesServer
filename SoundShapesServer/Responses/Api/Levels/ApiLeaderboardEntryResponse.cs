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
        User = new ApiUserResponse(entry.User);
        PlayTime = entry.PlayTime;
        Tokens = entry.Tokens;
        Deaths = entry.Deaths;
        Completed = entry.Completed;
        Date = entry.Date;
    }

    public string Id { get; set; }
    public string LevelId { get; set; }
    public int Position { get; set; }
    public ApiUserResponse User { get; set; }
    public long PlayTime { get; set; }
    public int Tokens { get; set; }
    public int Deaths { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset Date { get; set; }
}