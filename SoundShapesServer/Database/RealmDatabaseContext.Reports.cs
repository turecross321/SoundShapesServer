using Bunkum.CustomHttpListener.Parsing;
using MongoDB.Bson;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<Report> GetReports()
    {
        return this._realm.All<Report>();
    }
    public Report? GetReportWithId(string id)
    {
        return this._realm.All<Report>().FirstOrDefault(r => r.Id == id);
    }

    public void RemoveReport(Report report)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(report);
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
        
        this._realm.Write((() =>
        {
            this._realm.Add(report);
        }));
    }
}