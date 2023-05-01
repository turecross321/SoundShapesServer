using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(IQueryable<LeaderboardEntry> entries, int from,
        int count, LeaderboardOrderType order, bool descending, bool onlyBest, bool? completed, string? onLevel, string? byUser)
    {
        IQueryable<LeaderboardEntry> orderedEntries = OrderEntries(entries, order);
        IQueryable<LeaderboardEntry> fullyOrderedEntries = descending ? orderedEntries.Reverse() : orderedEntries;
        IQueryable<LeaderboardEntry> filteredEntries =
            FilterEntries(fullyOrderedEntries, onLevel, byUser, completed, onlyBest);

        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(filteredEntries, from, count);

        Entries = paginatedEntries.Select(entry => new ApiLeaderboardEntryResponse(entry, GetEntryPlacement(filteredEntries, entry))).ToArray();
        Count = filteredEntries.Count();
    }

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}