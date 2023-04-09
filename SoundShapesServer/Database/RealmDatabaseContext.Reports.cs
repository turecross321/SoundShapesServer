using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public void SubmitReport(GameUser reporter, GameLevel level, int reportReasonId)
    {
        Report report = new ()
        {
            Reporter = reporter,
            Level = level,
            ReportReasonId = reportReasonId
        };
        
        this._realm.Write((() =>
        {
            this._realm.Add(report);
        }));
    }
}