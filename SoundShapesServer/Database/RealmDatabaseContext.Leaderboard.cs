using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public bool SubmitScore(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new()
        {
            Id = GenerateGuid(),
            User = user,
            LevelId = levelId,
            Score = request.Score,
            PlayTime = request.PlayTime,
            Deaths = request.Deaths,
            Golded = request.Golded,
            Tokens = request.TokenCount,
            Completed = Convert.ToBoolean(request.Completed),
            Date = DateTimeOffset.UtcNow
        };

        LeaderboardEntry? previousEntry =
            this._realm.All<LeaderboardEntry>().FirstOrDefault(e => e.LevelId == levelId && e.User == user && e.Completed);

        this._realm.Write(() =>
        {
            // If there is a previous entry, and it's more than the new one, remove it and replace it with the new one
            if (previousEntry != null && previousEntry.Score > entry.Score)
            {
                this._realm.Remove(previousEntry);
                this._realm.Add(entry);
            }
            else if (previousEntry == null)
            {
                this._realm.Add(entry);
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
        IEnumerable<LeaderboardEntry> entries = this._realm.All<LeaderboardEntry>()
            .AsEnumerable()
            .Where(e=>e.Completed)
            .Where(e => e.LevelId == levelId)
            .OrderBy(e=>e.Score);

        return entries.AsQueryable();
    }
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(entry);
        });
    }
}