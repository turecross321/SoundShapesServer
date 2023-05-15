using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelsWrapper
{
    public ApiDailyLevelsWrapper(IEnumerable<DailyLevel> dailyLevels, int totalLevels)
    {
        DailyLevels = dailyLevels.Select(t => new ApiDailyLevelResponse(t)).ToArray();
        Count = totalLevels;
    }

    public ApiDailyLevelResponse[] DailyLevels { get; }
    public int Count { get; }
}