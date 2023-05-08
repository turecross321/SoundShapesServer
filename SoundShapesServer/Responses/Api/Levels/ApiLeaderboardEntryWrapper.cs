using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries)
    {
        Entries = paginatedEntries.Select(entry => new ApiLeaderboardEntryResponse(entry, GetEntryPlacement(allEntries, entry) + 1)).ToArray();
        Count = allEntries.Count();
    }     

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}