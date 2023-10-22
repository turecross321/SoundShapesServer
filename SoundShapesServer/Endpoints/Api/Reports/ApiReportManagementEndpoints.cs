using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Reports;

public class ApiReportManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("reports/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Deletes report with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.ReportNotFoundWhen)]
    public ApiOkResponse RemoveReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Report? report = database.GetReportWithId(id);
        if (report == null) 
            return ApiNotFoundError.ReportNotFound;
        
        database.RemoveReport(report);
        return new ApiOkResponse();
    }

    [ApiEndpoint("reports/id/{id}")]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Retrieves report with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.ReportNotFoundWhen)]
    public ApiResponse<ApiReportResponse> GetReport(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Report? report = database.GetReportWithId(id);
        if (report == null)
            return ApiNotFoundError.ReportNotFound;

        return new ApiReportResponse(database, report);
    }

    [ApiEndpoint("reports")]
    [DocUsesPageData]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Lists reports.")]
    public ApiListResponse<ApiReportResponse> GetReports(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        (int from, int count, bool descending) = context.GetPageData();
        
        ReportContentType? contentType = context.QueryString["contentType"].ToEnum<ReportContentType>();
        ReportReasonType? reasonType = context.QueryString["reasonType"].ToEnum<ReportReasonType>();
        GameUser? contentUser = context.QueryString["userId"].ToUser(database);
        GameLevel? contentLevel = context.QueryString["levelId"].ToLevel(database);
        LeaderboardEntry? contentLeaderboardEntry =
            context.QueryString["leaderboardEntryId"].ToLeaderboardEntry(database);

        ReportFilters filters = new ReportFilters
        {
            ContentType = contentType,
            ReasonType = reasonType,
            ContentUser = contentUser,
            ContentLevel = contentLevel,
            ContentLeaderboardEntry = contentLeaderboardEntry
        };

        (Report[] reports, int totalReports) = database.GetPaginatedReports(ReportOrderType.Date, descending, filters, from, count);
        return new ApiListResponse<ApiReportResponse>(reports.Select(r=>new ApiReportResponse(database, r)), totalReports);
    }
}