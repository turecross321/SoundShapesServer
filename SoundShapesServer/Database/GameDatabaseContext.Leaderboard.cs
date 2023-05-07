using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public void SubmitScore(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new (GenerateGuid(), user, levelId, request);

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });
        
        CreateEvent(user, EventType.ScoreSubmission, null, null, entry);
    }

    // TODO: Implement same ordering system as levels
    public IQueryable<LeaderboardEntry> GetLeaderboardEntries()
    {
        return _realm.All<LeaderboardEntry>();
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntriesOnLevel(string levelId)
    // Leaderboard Entries use ids instead of level objects, so they support campaign levels too
    {
        return _realm.All<LeaderboardEntry>()            
            .AsEnumerable()
            .Where(e=>e.LevelId == levelId)
            .AsQueryable();
    }

    public LeaderboardEntry? GetLeaderboardWithId(string id)
    {
        return _realm.All<LeaderboardEntry>().FirstOrDefault(e => e.Id == id);
    }
    
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}