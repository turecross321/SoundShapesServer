namespace SoundShapesServer.Responses.Api;

public class ApiLeaderboardEntryResponse
{
    public int Position { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    
    public long Score { get; set; }
    public long PlayTime { get; set; }
    public int Tokens { get; set; }
    public int Deaths { get; set; }
    
    public DateTimeOffset Date { get; set; }
}