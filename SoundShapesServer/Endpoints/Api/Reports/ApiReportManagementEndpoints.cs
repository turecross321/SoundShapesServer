using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Types;
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

        return new ApiReportResponse(report);
    }

    [ApiEndpoint("reports")]
    [DocUsesPageData]
    [DocUsesFiltration<ReportFilters>]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Lists reports.")]
    public ApiListResponse<ApiReportResponse> GetReports(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        (int from, int count, bool descending) = context.GetPageData();

        ReportFilters filters = context.GetFilters<ReportFilters>(database);
        ReportOrderType order = context.GetOrderType<ReportOrderType>() ?? ReportOrderType.CreationDate;

        (Report[] reports, int totalReports) = database.GetPaginatedReports(order, descending, filters, from, count);
        return new ApiListResponse<ApiReportResponse>(reports.Select(r=>new ApiReportResponse(r)), totalReports);
    }
}