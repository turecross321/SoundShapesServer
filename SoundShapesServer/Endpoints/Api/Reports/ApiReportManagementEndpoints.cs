using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Reports;

public class ApiReportManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("reports/id/{id}/remove", Method.Post)]
    public Response RemoveReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;
        
        database.RemoveReport(report);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("reports/id/{id}")]
    public Response GetReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;

        return new Response(new ApiReportResponse(database, report), ContentType.Json);
    }

    [ApiEndpoint("reports")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public ApiReportsWrapper? GetReports(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return null;
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");

        string? contentTypeString = context.QueryString["contentType"];
        ReportContentType? contentType = null;
        
        if (contentTypeString != null)
        {
            if (Enum.TryParse(contentTypeString, out ReportContentType type))
                contentType = type;
        }
        
        string? reasonTypeString = context.QueryString["reasonType"];
        ReportReasonType? reasonType = null;
        
        if (reasonTypeString != null)
        {
            if (Enum.TryParse(reasonTypeString, out ReportReasonType type))
                reasonType = type;
        }
        
        string? contentUserId = context.QueryString["levelId"];
        string? contentLevelId = context.QueryString["userId"];
        string? contentLeaderboardEntryId = context.QueryString["leaderboardEntryId"];

        GameUser? contentUser = null;
        GameLevel? contentLevel = null;
        LeaderboardEntry? contentLeaderboardEntry = null;

        if (contentUserId != null)
            contentUser = database.GetUserWithId(contentUserId);
        if (contentLevelId != null)
            contentLevel = database.GetLevelWithId(contentLevelId);
        if (contentLeaderboardEntryId != null)
            contentLeaderboardEntry = database.GetLeaderboardEntryWithId(contentLeaderboardEntryId);

        ReportFilters filters = new (contentType, reasonType, contentUser, contentLevel, contentLeaderboardEntry);

        (Report[] reports, int totalReports) = database.GetReports(ReportOrderType.Date, descending, filters, from, count);
        return new ApiReportsWrapper(database, reports, totalReports);
    }
}