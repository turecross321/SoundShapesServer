using Newtonsoft.Json;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Reports;

namespace SoundShapesServer.Responses.Api.Responses.Moderation;

public class ApiReportResponse : IApiResponse, IDataConvertableFrom<ApiReportResponse, Report>
{
    public required string Id { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ApiUserBriefResponse? ContentUser { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ApiLevelBriefResponse? ContentLevel { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }

    public required ReportContentType ContentType { get; set; }
    public required ReportReasonType ReasonType { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required ApiUserBriefResponse Author { get; set; }

    public static ApiReportResponse FromOld(Report old)
    {
        ApiUserBriefResponse? contentUser = null;
        if (old.ContentUser != null)
            contentUser = ApiUserBriefResponse.FromOld(old.ContentUser);

        ApiLevelBriefResponse? contentLevel = null;
        if (old.ContentLevel != null)
            contentLevel = ApiLevelBriefResponse.FromOld(old.ContentLevel);

        LeaderboardFilters filters = new()
        {
            Completed = old.ContentLeaderboardEntry?.Completed,
            Obsolete = old.ContentLeaderboardEntry?.Obsolete()
        };

        ApiLeaderboardEntryResponse? contentLeaderboardEntry = null;
        if (old.ContentLeaderboardEntry != null)
            contentLeaderboardEntry =
                ApiLeaderboardEntryResponse.FromOld(old.ContentLeaderboardEntry, LeaderboardOrderType.Score, filters);

        return new ApiReportResponse
        {
            Id = old.Id.ToString()!,
            ContentUser = contentUser,
            ContentLevel = contentLevel,
            ContentLeaderboardEntry = contentLeaderboardEntry,
            ContentType = old.ContentType,
            ReasonType = old.ReasonType,
            CreationDate = old.CreationDate,
            Author = ApiUserBriefResponse.FromOld(old.Author)
        };
    }

    public static IEnumerable<ApiReportResponse> FromOldList(IEnumerable<Report> oldList)
    {
        return oldList.Select(FromOld);
    }
}