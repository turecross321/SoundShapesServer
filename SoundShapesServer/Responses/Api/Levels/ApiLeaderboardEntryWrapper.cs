using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(IReadOnlyList<LeaderboardEntry> paginatedEntries, int totalEntries, int from, bool descending)
    {
        Entries = new ApiLeaderboardEntryResponse[paginatedEntries.Count];
        
        for (int i = 0; i < paginatedEntries.Count; i++)
        {
            Entries[i] = new ApiLeaderboardEntryResponse(paginatedEntries[i], CalculateEntryPlacement(totalEntries, from, i, descending, false));
        }

        Count = totalEntries;
    }     

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}