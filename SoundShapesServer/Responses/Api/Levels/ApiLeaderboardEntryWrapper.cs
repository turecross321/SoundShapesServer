using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(LeaderboardEntry[] paginatedEntries, int totalEntries, int from, bool descending)
    {
        Entries = new ApiLeaderboardEntryResponse[paginatedEntries.Length];
        
        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            Entries[i] = new ApiLeaderboardEntryResponse(paginatedEntries[i], CalculateEntryPlacement(totalEntries, from, i, descending, false));
        }

        Count = totalEntries;
    }     

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}