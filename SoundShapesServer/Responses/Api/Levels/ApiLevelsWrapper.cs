using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelsWrapper
{
    public ApiLevelsWrapper(GameLevel[] levels, int count)
    {
        Levels = levels.Select(l=> new ApiLevelSummaryResponse(l)).ToArray();
        Count = count;
    }

    public ApiLevelSummaryResponse[] Levels { get; set; }
    public int Count { get; set; }
}