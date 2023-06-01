using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelsWrapper
{
    public ApiLevelsWrapper(GameLevel[] levels, int count)
    {
        Levels = levels.Select(l=> new ApiLevelBriefResponse(l)).ToArray();
        Count = count;
    }
    
#pragma warning disable CS8618
    public ApiLevelsWrapper() {}
#pragma warning restore CS8618

    public ApiLevelBriefResponse[] Levels { get; set; }
    public int Count { get; set; }
}