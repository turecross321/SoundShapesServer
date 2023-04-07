using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Leaderboards;

public class LeaderboardEntriesResponse
{
    public LeaderboardEntryResponse[] items { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? previousToken { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? nextToken { get; set; }
}