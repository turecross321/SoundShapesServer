namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelsWrapper
{
    public ApiDailyLevelsWrapper(ApiDailyLevelResponse[] dailyLevels, int count)
    {
        DailyLevels = dailyLevels;
        Count = count;
    }

    public ApiDailyLevelResponse[] DailyLevels { get; }
    public int Count { get; }
}