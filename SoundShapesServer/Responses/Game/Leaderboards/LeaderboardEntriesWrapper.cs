using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntriesWrapper
{
    public LeaderboardEntriesWrapper(IQueryable<LeaderboardEntry> entries, int from, int count, LeaderboardOrderType order, string onLevel)
    {
        IQueryable<LeaderboardEntry> orderedEntries = OrderEntries(entries, order, false);
        IQueryable<LeaderboardEntry> filteredEntries = FilterEntries(orderedEntries, onLevel, null, true, true);

        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(entries.Count(), from, count);
        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(filteredEntries, from, count);

        Entries = paginatedEntries.Select(entry => new LeaderboardEntryResponse(entry, GetEntryPlacement(filteredEntries, entry) + 1)).ToArray();
        NextToken = nextToken;
        PreviousToken = previousToken;
    }

    [JsonProperty("items")] public LeaderboardEntryResponse[] Entries { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}