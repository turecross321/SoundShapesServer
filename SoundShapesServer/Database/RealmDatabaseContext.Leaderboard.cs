using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public bool SubmitScore(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new (GenerateGuid(), user, levelId, request);

        LeaderboardEntry? previousEntry =
            _realm.All<LeaderboardEntry>().AsEnumerable().FirstOrDefault(e => e.LevelId == levelId && e.User == user && e.Completed);

        _realm.Write(() =>
        {
            // If there is a previous entry, and it's more than the new one, remove it and replace it with the new one
            if (previousEntry != null && previousEntry.Score > entry.Score)
            {
                _realm.Remove(previousEntry);
                _realm.Add(entry);
            }
            else if (previousEntry == null)
            {
                _realm.Add(entry);
            }
            else
            {
                Console.WriteLine("not submitting score");
            }
        });

        return true;
    }

    public IQueryable<LeaderboardEntry> GetLeaderboardEntries(string levelId)
    {
        IEnumerable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>()
            .AsEnumerable()
            .Where(e=>e.Completed)
            .Where(e => e.LevelId == levelId)
            .OrderBy(e=>e.Score);

        return entries.AsQueryable();
    }
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}