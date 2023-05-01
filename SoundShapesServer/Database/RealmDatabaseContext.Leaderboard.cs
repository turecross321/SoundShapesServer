using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public bool SubmitScore(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new (GenerateGuid(), user, levelId, request);

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });

        return true;
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntries()
    {
        return _realm.All<LeaderboardEntry>();
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntriesOnLevel(string levelId)
    {
        return _realm.All<LeaderboardEntry>()            
            .AsEnumerable()
            .Where(e=>e.LevelId == levelId)
            .AsQueryable();
    }


    // todo: moderators should be able to calkl this
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}