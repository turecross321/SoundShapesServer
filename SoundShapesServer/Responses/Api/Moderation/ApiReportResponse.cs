using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Reports;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportResponse
{
    public ApiReportResponse(GameDatabaseContext database, Report report)
    {
        Id = report.Id;
        if (report.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(report.ContentUser);
        if (report.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(report.ContentLevel);
        if (report.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(report.ContentLeaderboardEntry, database.GetLeaderboardEntryPosition(report.ContentLeaderboardEntry));
        ContentType = report.ContentType;
        ReasonType = report.ReasonType;
        CreationDate = report.CreationDate;
        Author = new ApiUserBriefResponse(report.Author);
    }

    public string Id { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public ReportContentType ContentType { get; set; }
    public ReportReasonType ReasonType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public ApiUserBriefResponse Author { get; set; }
}