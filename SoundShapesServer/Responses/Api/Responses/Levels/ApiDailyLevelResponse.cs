using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiDailyLevelResponse : IApiResponse
{
    public ApiDailyLevelResponse(DailyLevel dailyLevel)
    {
        Id = dailyLevel.Id;
        Level = new ApiLevelBriefResponse(dailyLevel.Level);
        Date = dailyLevel.Date;
        CreationDate = dailyLevel.CreationDate;
        ModificationDate = dailyLevel.ModificationDate;
        Author = new ApiUserBriefResponse(dailyLevel.Author);
    }

    public string Id { get; }
    public ApiLevelBriefResponse Level { get; }
    public DateTimeOffset Date { get; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset ModificationDate { get; }
    public ApiUserBriefResponse Author { get; set; }
}