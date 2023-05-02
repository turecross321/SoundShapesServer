using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelResponseWrapper
{
    public ApiLevelResponseWrapper(IQueryable<GameLevel> levels, int from, int count, GameUser? user, LevelOrderType order, bool descending)
    {
        IQueryable<GameLevel> orderedLevels = LevelHelper.OrderLevels(levels, order);
        IQueryable<GameLevel> fullyOrderedLevels = descending ? orderedLevels
            .AsEnumerable()
            .Reverse()
            .AsQueryable() : orderedLevels;

        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(fullyOrderedLevels, from, count);

        Levels = paginatedLevels.Select(l=> new ApiLevelSummaryResponse(l, user)).ToArray();
        Count = levels.Count();
    }

    public ApiLevelSummaryResponse[] Levels { get; set; }
    public int Count { get; set; }
}