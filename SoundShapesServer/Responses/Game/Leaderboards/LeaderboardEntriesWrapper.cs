using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntriesWrapper
{
    public LeaderboardEntriesWrapper(IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries, int from, int count)
    {
        Entries = paginatedEntries.Select(entry => new LeaderboardEntryResponse(entry, GetEntryPlacement(allEntries, entry) + 1)).ToArray();
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(allEntries.Count(), from, count);
    }        

    [JsonProperty("items")] public LeaderboardEntryResponse[] Entries { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}