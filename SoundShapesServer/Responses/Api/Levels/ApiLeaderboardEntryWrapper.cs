namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}