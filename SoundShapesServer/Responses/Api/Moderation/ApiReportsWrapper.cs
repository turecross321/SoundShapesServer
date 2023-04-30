using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportsWrapper
{
    public ApiReportsWrapper(IQueryable<Report> reports, int from, int count)
    {
        Report[] paginatedReports = PaginationHelper.PaginateReports(reports, from, count);

        List<ApiReportResponse> reportResponses = new ();

        for (int i = 0; i < paginatedReports.Length; i++)
        {
            ApiReportResponse reportResponse = new (paginatedReports[i]);
            reportResponses.Add(reportResponse);
        }

        Reports = reportResponses.ToArray();
        Count = reports.Count();
    }

    public ApiReportResponse[] Reports { get; set; }
    public int Count { get; set; }
}