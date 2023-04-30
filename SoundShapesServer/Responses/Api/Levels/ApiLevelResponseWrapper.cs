using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelResponseWrapper
{
    public ApiLevelResponseWrapper(IQueryable<GameLevel> levels, int from, int count, GameUser? user)
    {
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(levels, from, count);
        
        ApiLevelResponse[] levelResponses = new ApiLevelResponse[paginatedLevels.Length];
        
        for (int i = 0; i < paginatedLevels.Length; i++)
        {
            levelResponses[i] = new ApiLevelResponse(paginatedLevels[i], user);
        }

        Levels = levelResponses;
        Count = levels.Count();
    }

    public ApiLevelResponse[] Levels { get; set; }
    public int Count { get; set; }
}