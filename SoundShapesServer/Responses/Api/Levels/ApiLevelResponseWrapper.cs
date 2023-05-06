using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelResponseWrapper
{
    public ApiLevelResponseWrapper(IQueryable<GameLevel> levels, int from, int count, LevelOrderType order, bool descending)
    {
        IQueryable<GameLevel> orderedLevels = LevelHelper.OrderLevels(levels, order, descending);

        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(orderedLevels, from, count);

        Levels = paginatedLevels.Select(l=> new ApiLevelSummaryResponse(l)).ToArray();
        Count = levels.Count();
    }

    public ApiLevelSummaryResponse[] Levels { get; set; }
    public int Count { get; set; }
}