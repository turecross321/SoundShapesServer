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
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(report.ContentLeaderboardEntry, database.GetEntryPlacement(report.ContentLeaderboardEntry));
        ContentType = report.ContentType;
        ReasonType = report.ReasonType;
        Date = report.Date;
        Issuer = new ApiUserBriefResponse(report.Issuer);
    }

    public string Id { get; set; }
    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public int ContentType { get; set; }
    public int ReasonType { get; set; }
    public DateTimeOffset Date { get; set; }
    public ApiUserBriefResponse Issuer { get; set; }
}