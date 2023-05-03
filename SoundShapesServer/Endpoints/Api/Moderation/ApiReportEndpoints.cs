using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiReportEndpoints : EndpointGroup
{
    [ApiEndpoint("reports/{id}/remove", Method.Post)]
    public Response RemoveReport(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;
        
        database.RemoveReport(report);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("reports/{id}")]
    public Response GetReport(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        Report? report = database.GetReportWithId(id);
        if (report == null) return HttpStatusCode.NotFound;

        return new Response(new ApiReportResponse(report), ContentType.Json);
    }

    [ApiEndpoint("reports")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public ApiReportsWrapper? GetReports(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return null;
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? contentId = context.QueryString["contentId"];
        string? contentType = context.QueryString["contentType"];

        IQueryable<Report> reports = database.GetReports();
        IQueryable<Report> filteredReports = ReportHelper.FilterReports(reports, contentId, contentType);
        IQueryable<Report> orderedReports =
            descending ? filteredReports.AsEnumerable().Reverse().AsQueryable() : filteredReports;
        
        return new ApiReportsWrapper(orderedReports, from, count);
    }
}