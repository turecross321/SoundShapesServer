using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportResponse : IApiResponse
{
    public ApiReportResponse(GameDatabaseContext database, Report report, GameUser accessor)
    {
        Id = report.Id;
        if (report.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(report.ContentUser);
        if (report.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(report.ContentLevel);
        if (report.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(report.ContentLeaderboardEntry, database.GetLeaderboardEntryPosition(report.ContentLeaderboardEntry, accessor));
        ContentType = report.ContentType;
        ReasonType = report.ReasonType;
        CreationDate = report.CreationDate.ToUnixTimeSeconds();
        Author = new ApiUserBriefResponse(report.Author);
    }

    public string Id { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public ReportContentType ContentType { get; set; }
    public ReportReasonType ReasonType { get; set; }
    public long CreationDate { get; set; }
    public ApiUserBriefResponse Author { get; set; }
}