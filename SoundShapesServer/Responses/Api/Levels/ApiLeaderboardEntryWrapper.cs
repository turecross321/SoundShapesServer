using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(IQueryable<LeaderboardEntry> entries, int from,
        int count)
    {
        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(entries, from, count);

        Entries = paginatedEntries.Select(entry => new ApiLeaderboardEntryResponse(entry, GetEntryPlacement(entries, entry))).ToArray();
        Count = entries.Count();
    }

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}