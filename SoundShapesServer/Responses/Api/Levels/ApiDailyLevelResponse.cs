using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id;
        Level = new ApiLevelSummaryResponse(dailyLevel.Level);
        Date = dailyLevel.Date;
        Artist = new ApiUserResponse(dailyLevel.Artist);
    }

    public string Id { get; }
    public ApiLevelSummaryResponse Level { get; }
    public DateTimeOffset Date { get; }
    public ApiUserResponse Artist { get; set; }
}