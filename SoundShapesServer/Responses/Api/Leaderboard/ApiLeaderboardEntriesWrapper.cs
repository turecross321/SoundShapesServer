using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Leaderboard;

public class ApiLeaderboardEntriesWrapper
{
    public ApiLeaderboardEntriesWrapper(IReadOnlyList<LeaderboardEntry> paginatedEntries, int totalEntries, int from, bool descending)
    {
        Entries = new ApiLeaderboardEntryResponse[paginatedEntries.Count];
        
        for (int i = 0; i < paginatedEntries.Count; i++)
        {
            Entries[i] = new ApiLeaderboardEntryResponse(paginatedEntries[i], CalculateEntryPlacement(totalEntries, from, i, descending, false));
        }

        Count = totalEntries;
    }
    
#pragma warning disable CS8618
    public ApiLeaderboardEntriesWrapper() {}
#pragma warning restore CS8618

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}