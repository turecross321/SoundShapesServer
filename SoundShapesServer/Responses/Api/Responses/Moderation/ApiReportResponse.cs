using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Reports;

namespace SoundShapesServer.Responses.Api.Responses.Moderation;

public class ApiReportResponse : IApiResponse
{
    public ApiReportResponse(GameDatabaseContext database, Report report)
    {
        Id = report.Id;
        if (report.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(report.ContentUser);
        if (report.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(report.ContentLevel);
        if (report.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(report.ContentLeaderboardEntry);
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