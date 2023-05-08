using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(LeaderboardEntry[] paginatedEntries, int totalEntries, int from, bool areEntriesDescending)
    {
        Entries = new ApiLeaderboardEntryResponse[paginatedEntries.Length];
        
        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            int position = areEntriesDescending ? totalEntries - (from + i) : from + i;
            Entries[i] = new ApiLeaderboardEntryResponse(paginatedEntries[i], position);
        }

        Count = totalEntries;
    }     

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}