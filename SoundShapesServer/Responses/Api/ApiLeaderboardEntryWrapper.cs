namespace SoundShapesServer.Responses.Api;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}