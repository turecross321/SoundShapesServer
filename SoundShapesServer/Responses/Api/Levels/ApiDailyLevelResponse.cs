using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id;
        Level = new ApiLevelSummaryResponse(dailyLevel.Level);
        DateUtc = dailyLevel.Date;
    }

    public string Id { get; }
    public ApiLevelSummaryResponse Level { get; }
    public DateTimeOffset DateUtc { get; }
}