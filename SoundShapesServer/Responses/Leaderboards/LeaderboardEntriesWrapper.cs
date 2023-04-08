using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Leaderboards;

public class LeaderboardEntriesWrapper
{
    public LeaderboardEntryResponse[] items { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? previousToken { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? nextToken { get; set; }
}