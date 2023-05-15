using SoundShapesServer.Database;
using SoundShapesServer.Types.Reports;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportsWrapper
{
    public ApiReportsWrapper(GameDatabaseContext database, IEnumerable<Report> reports, int totalReports)
    {
        Reports = reports.Select(r => new ApiReportResponse(database, r)).ToArray();
        Count = totalReports;
    }

    public ApiReportResponse[] Reports { get; set; }
    public int Count { get; set; }
}