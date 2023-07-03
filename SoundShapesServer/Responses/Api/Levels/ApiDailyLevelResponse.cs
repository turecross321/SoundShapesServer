using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
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