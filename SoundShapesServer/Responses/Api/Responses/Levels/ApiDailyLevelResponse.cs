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
        Date = dailyLevel.Date.ToUnixTimeSeconds();
        CreationDate = dailyLevel.CreationDate.ToUnixTimeSeconds();
        ModificationDate = dailyLevel.ModificationDate.ToUnixTimeSeconds();
        Author = new ApiUserBriefResponse(dailyLevel.Author);
    }

    public string Id { get; }
    public ApiLevelBriefResponse Level { get; }
    public long Date { get; }
    public long CreationDate { get; }
    public long ModificationDate { get; }
    public ApiUserBriefResponse Author { get; set; }
}