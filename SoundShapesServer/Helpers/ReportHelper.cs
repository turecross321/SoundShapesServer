using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ReportHelper
{
    public static ApiReportsWrapper ReportsToWrapper(IQueryable<Report> reports, int from, int count)
    {
        Report[] paginatedReports = PaginationHelper.PaginateReports(reports, from, count);

        List<ApiReportResponse> reportResponses = new ();

        for (int i = 0; i < paginatedReports.Length; i++)
        {
            ApiReportResponse reportResponse = ReportToResponse(paginatedReports[i]);
            reportResponses.Add(reportResponse);
        }

        ApiReportsWrapper response = new()
        {
            Reports = reportResponses.ToArray(),
            Count = reports.Count()
        };

        return response;
    }
    
    public static ApiReportResponse ReportToResponse(Report report)
    {
        return new ApiReportResponse
        {
            Id = report.Id,
            ContentId = report.ContentId,
            ContentType = report.ContentType,
            ReportReasonId = report.ReportReasonId,
            Issued = report.Issued,
            IssuerId = report.Issuer.Id
        };
    }
}