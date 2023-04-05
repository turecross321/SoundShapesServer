using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public bool SubmitScore(LeaderboardSubmissionRequest request, GameUser user, GameLevel level)
    {
        LeaderboardEntry entry = new()
        {
            user = user,
            level = level,
            score = request.score,
            playTime = request.playTime,
            deaths = request.deaths,
            golded = Convert.ToBoolean(request.golded),
            tokenCount = request.tokenCount,
            completed = Convert.ToBoolean(request.completed),
            date = DateTimeOffset.UtcNow
        };

        this._realm.Write(() =>
        {
            this._realm.Add(entry);
        });

        return true;
    }
}