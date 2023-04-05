namespace SoundShapesServer.Responses.Leaderboards;

public class LeaderboardEntryResponse
{
    public int position { get; set; }
    public UserResponse entrant { get; set; }
    public long score { get; set; }
}