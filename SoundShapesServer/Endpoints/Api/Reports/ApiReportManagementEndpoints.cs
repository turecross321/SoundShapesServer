using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Reports;

public class ApiReportManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("reports/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Deletes report with specified ID.")]
    public Response RemoveReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;
        
        database.RemoveReport(report);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("reports/id/{id}")]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Retrieves report with specified ID.")]
    public Response GetReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;

        return new Response(new ApiReportResponse(database, report), ContentType.Json);
    }

    [ApiEndpoint("reports")]
    [DocUsesPageData]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Lists reports.")]
    public ApiListResponse<ApiReportResponse> GetReports(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

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
        return new ApiListResponse<ApiReportResponse>(reports.Select(r=>new ApiReportResponse(database, r)), totalReports);
    }
}