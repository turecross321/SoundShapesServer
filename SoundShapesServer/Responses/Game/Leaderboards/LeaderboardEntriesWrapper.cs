using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntriesWrapper
{
    public LeaderboardEntriesWrapper(IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries, int from, int count, bool areEntriesDescending)
    {
        Entries = new LeaderboardEntryResponse[paginatedEntries.Length];
        
        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            int position = areEntriesDescending ? allEntries.Count() - (from + i) : from + i;
            Entries[i] = new LeaderboardEntryResponse(paginatedEntries[i], position);
        }
        
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(allEntries.Count(), from, count);
    }        

    [JsonProperty("items")] public LeaderboardEntryResponse[] Entries { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}