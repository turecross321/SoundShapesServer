using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelResponseWrapper
{
    public ApiLevelResponseWrapper(IQueryable<GameLevel> levels, int from, int count, GameUser? user, LevelOrderType order, bool descending)
    {
        IQueryable<GameLevel> orderedLevels = LevelHelper.OrderLevels(levels, order);
        IQueryable<GameLevel> fullyOrderedLevels = descending ? orderedLevels.OrderDescending() : orderedLevels;

        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(fullyOrderedLevels, from, count);
        
        ApiLevelSummaryResponse[] levelResponses = new ApiLevelSummaryResponse[paginatedLevels.Length];
        
        for (int i = 0; i < paginatedLevels.Length; i++)
        {
            levelResponses[i] = new ApiLevelSummaryResponse(paginatedLevels[i], user);
        }

        Levels = levelResponses;
        Count = levels.Count();
    }

    public ApiLevelSummaryResponse[] Levels { get; set; }
    public int Count { get; set; }
}