using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id;
        Level = new ApiLevelSummaryResponse(dailyLevel.Level);
        Date = dailyLevel.Date;
        Author = new ApiUserResponse(dailyLevel.Author);
    }

    public string Id { get; }
    public ApiLevelSummaryResponse Level { get; }
    public DateTimeOffset Date { get; }
    public ApiUserResponse Author { get; set; }
}