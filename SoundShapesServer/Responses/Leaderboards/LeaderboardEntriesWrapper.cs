using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Leaderboards;

public class LeaderboardEntriesWrapper
{
    [JsonProperty("items")] public LeaderboardEntryResponse[] Entries { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}