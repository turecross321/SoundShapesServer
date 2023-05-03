using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ReportHelper
{
    public static IQueryable<Report> FilterReports(IQueryable<Report> reports, string? contentId, string? contentType)
    {
        IQueryable<Report> response = reports;

        if (contentId != null)
        {
            response = reports.Where(r => r.ContentId == contentId);
        }

        if (contentType != null)
        {
            response = response.Where(r => r.ContentType == contentType);
        }

        return response;
    }
}