using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryResponse
{
    public ApiLeaderboardEntryResponse(LeaderboardEntry entry, int position)
    {
        Id = entry.Id;
        LevelId = entry.LevelId;
        Position = position;

        UserId = entry.User.Id;
        Username = entry.User.Username;
        
        PlayTime = entry.PlayTime;
        Tokens = entry.Tokens;
        Deaths = entry.Deaths;
        Completed = entry.Completed;
        Date = entry.Date;
    }

    public string Id { get; set; }
    public string LevelId { get; set; }
    public int Position { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    
    public long PlayTime { get; set; }
    public int Tokens { get; set; }
    public int Deaths { get; set; }
    public bool Completed { get; set; }
    public DateTimeOffset Date { get; set; }
}