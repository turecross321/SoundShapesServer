using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiDailyLevelResponse : IApiResponse, IDataConvertableFrom<ApiDailyLevelResponse, DailyLevel>
{
    public required string Id { get; set; }
    public required ApiLevelBriefResponse Level { get; set; }
    public required DateTimeOffset Date { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }
    public required ApiUserBriefResponse Author { get; set; }

    public static ApiDailyLevelResponse FromOld(DailyLevel old)
    {
        return new ApiDailyLevelResponse
        {
            Id = old.Id.ToString()!,
            Level = ApiLevelBriefResponse.FromOld(old.Level),
            Date = old.Date,
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            Author = ApiUserBriefResponse.FromOld(old.Author)
        };
    }

    public static IEnumerable<ApiDailyLevelResponse> FromOldList(IEnumerable<DailyLevel> oldList)
    {
        return oldList.Select(FromOld);
    }
}