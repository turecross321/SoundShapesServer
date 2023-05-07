using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Leaderboard;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLeaderboardEntryWrapper
{
    public ApiLeaderboardEntryWrapper(IQueryable<LeaderboardEntry> entries, int from,
        int count, LeaderboardOrderType order, bool descending, bool onlyBest, bool? completed, string? onLevel, string? byUser)
    {
        IQueryable<LeaderboardEntry> orderedEntries = OrderEntries(entries, order, descending);
        IQueryable<LeaderboardEntry> filteredEntries =
            FilterEntries(orderedEntries, onLevel, byUser, completed, onlyBest);

        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(filteredEntries, from, count);

        Entries = paginatedEntries.Select(entry => new ApiLeaderboardEntryResponse(entry, GetEntryPlacement(filteredEntries, entry))).ToArray();
        Count = filteredEntries.Count();
    }

    public ApiLeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}