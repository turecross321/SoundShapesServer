using SoundShapesServer.Types.Reports;

namespace SoundShapesServer.Extensions.Queryable;

public static class ReportQueryableExtensions
{
    public static IQueryable<Report> FilterReports(this IQueryable<Report> reports, ReportFilters filters)
    {
        if (filters.ContentUser != null)
        {
            reports = reports.Where(r => r.ContentUser == filters.ContentUser);
        }
        
        if (filters.ContentLevel != null)
        {
            reports = reports.Where(r => r.ContentLevel == filters.ContentLevel);
        }
        
        if (filters.ContentLeaderboardEntry != null)
        {
            reports = reports.Where(r => r.ContentLeaderboardEntry == filters.ContentLeaderboardEntry);
        }

        if (filters.ContentType != null)
        {
            reports = reports.Where(r => r._ContentType == (int)filters.ContentType);
        }

        if (filters.ReasonType != null)
        {
            reports = reports.Where(r => r._ReasonType == (int)filters.ReasonType);
        }

        return reports;
    }
    
    public static IQueryable<Report> OrderReports(this IQueryable<Report> reports, ReportOrderType order, bool descending)
    {
        return order switch
        {
            ReportOrderType.CreationDate => reports.OrderByDynamic(r => r.CreationDate, descending),
            _ => reports.OrderReports(ReportOrderType.CreationDate, descending)
        };
    }
}