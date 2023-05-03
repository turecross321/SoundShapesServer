using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportsWrapper
{
    public ApiReportsWrapper(IQueryable<Report> reports, int from, int count)
    {
        Report[] paginatedReports = PaginationHelper.PaginateReports(reports, from, count);

        Reports = paginatedReports.Select(t => new ApiReportResponse(t)).ToArray();
        Count = reports.Count();
    }

    public ApiReportResponse[] Reports { get; set; }
    public int Count { get; set; }
}