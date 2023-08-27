using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public void RemoveReport(Report report)
    {
        _realm.Write(() =>
        {
            _realm.Remove(report);
        });
    }

    private void RemoveAllReportsWithContentUser(GameUser user)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(_realm.All<Report>().Where(r=>r.ContentUser == user));
        });
    }
    private void RemoveAllReportsWithContentLevel(GameLevel level)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(_realm.All<Report>().Where(r=>r.ContentLevel == level));
        });
    }
    private void RemoveAllReportsWithContentLeaderboardEntry(LeaderboardEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(_realm.All<Report>().Where(r=>r.ContentLeaderboardEntry == entry));
        });
    }

    public void CreateReport(GameUser reporter, ReportContentType contentType, ReportReasonType reportReasonType, 
        GameUser? contentUser = null, GameLevel? contentLevel = null, LeaderboardEntry? contentLeaderboardEntry = null)
    {
        Report report = new()
        {
            Id = GenerateGuid(),            
            Author = reporter,
            ContentType = contentType,
            ContentUser = contentUser,
            ContentLevel = contentLevel,
            ContentLeaderboardEntry = contentLeaderboardEntry,
            ReasonType = reportReasonType,
            CreationDate = DateTimeOffset.UtcNow
        };
        
        _realm.Write(() =>
        {
            _realm.Add(report);
        });
    }
    
    public Report? GetReportWithId(string id)
    {
        return _realm.All<Report>().FirstOrDefault(r => r.Id == id);
    }
    
    public (Report[], int) GetPaginatedReports(ReportOrderType order, bool descending, ReportFilters filters, int from, int count)
    {
        IQueryable<Report> orderedReports = GetReports(order, descending, filters);
        Report[] paginatedReports = PaginateReports(orderedReports, from, count);
        
        return (paginatedReports, orderedReports.Count());
    }

    private IQueryable<Report> GetReports(ReportOrderType order, bool descending, ReportFilters filters)
    {
        IQueryable<Report> reports = _realm.All<Report>();
        IQueryable<Report> filteredReports = FilterReports(reports, filters);
        IQueryable<Report> orderedReports = OrderReports(filteredReports, order, descending);

        return orderedReports;
    }

    private static IQueryable<Report> FilterReports(IQueryable<Report> reports, ReportFilters filters)
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

    #region Report Ordering

    private static IQueryable<Report> OrderReports(IQueryable<Report> reports, ReportOrderType order, bool descending)
    {
        return order switch
        {
            ReportOrderType.Date => OrderReportsByDate(reports, descending),
            _ => reports
        };
    }
    
    private static IQueryable<Report> OrderReportsByDate(IQueryable<Report> reports, bool descending)
    {
        if (descending) return reports.OrderByDescending(r => r.CreationDate);
        return reports.OrderBy(r => r.CreationDate);
    }
    
    #endregion
}