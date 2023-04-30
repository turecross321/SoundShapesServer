using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<Report> GetReports()
    {
        return _realm.All<Report>();
    }
    public Report? GetReportWithId(string id)
    {
        return _realm.All<Report>().FirstOrDefault(r => r.Id == id);
    }

    public void RemoveReport(Report report)
    {
        _realm.Write(() =>
        {
            _realm.Remove(report);
        });
    } 
    public void SubmitReport(GameUser reporter, string contentId, ServerContentType contentType, int reportReasonId)
    {
        Report report = new ()
        {
            Id = GenerateGuid(),            
            Issuer = reporter,
            ContentId = contentId,
            ContentType = (int)contentType,
            ReportReasonId = reportReasonId,
            Issued = DateTimeOffset.UtcNow
        };
        
        _realm.Write(() =>
        {
            _realm.Add(report);
        });
    }
}