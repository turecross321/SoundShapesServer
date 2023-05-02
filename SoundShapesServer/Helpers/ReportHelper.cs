using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ReportHelper
{
    public static IQueryable<Report> FilterReports(IQueryable<Report> reports, string? contentId)
    {
        IQueryable<Report> response = reports;

        if (contentId != null)
        {
            response = reports.Where(r => r.ContentId == contentId);
        }

        return response;
    }
}