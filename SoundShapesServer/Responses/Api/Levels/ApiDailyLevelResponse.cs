using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id;
        Level = new ApiLevelBriefResponse(dailyLevel.Level);
        Date = dailyLevel.Date;
        Author = new ApiUserBriefResponse(dailyLevel.Author);
    }

    public string Id { get; }
    public ApiLevelBriefResponse Level { get; }
    public DateTimeOffset Date { get; }
    public ApiUserBriefResponse Author { get; set; }
}