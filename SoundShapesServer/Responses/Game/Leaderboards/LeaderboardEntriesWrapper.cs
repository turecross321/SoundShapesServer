using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntriesWrapper
{
    public LeaderboardEntriesWrapper(IQueryable<LeaderboardEntry> entries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(entries.Count(), from, count);
        
        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(entries, from, count);

        List<LeaderboardEntryResponse> entryResponses = new ();

        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            entryResponses.Add(new LeaderboardEntryResponse(paginatedEntries[i], i + from + 1));
        }

        Entries = entryResponses.ToArray();
        NextToken = nextToken;
        PreviousToken = previousToken;
    }

    [JsonProperty("items")] public LeaderboardEntryResponse[] Entries { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}