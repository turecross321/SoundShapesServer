using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id ?? "";
        LevelId = dailyLevel.Level?.Id ?? "";
        DateUtc = dailyLevel.Date;
    }

    public string Id { get; }
    public string LevelId { get; }
    public DateTimeOffset DateUtc { get; }
}