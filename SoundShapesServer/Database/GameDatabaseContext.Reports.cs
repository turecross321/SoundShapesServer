using MongoDB.Bson;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

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
        if (!ObjectId.TryParse(id, out ObjectId objectId)) 
            return null;
        
        return _realm.All<Report>().FirstOrDefault(r => r.Id == objectId);
    }
    
    public (Report[], int) GetPaginatedReports(ReportOrderType order, bool descending, ReportFilters filters, int from, int count)
    {
        return _realm.All<Report>().FilterReports(filters).OrderReports(order, descending).Paginate(from, count);
    }
}