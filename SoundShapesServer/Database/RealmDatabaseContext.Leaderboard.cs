using MongoDB.Bson.Serialization.Conventions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Leaderboards;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public bool SubmitScore(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new()
        {
            user = user,
            levelId = levelId,
            score = request.score,
            playTime = request.playTime,
            deaths = request.deaths,
            golded = request.golded,
            tokenCount = request.tokenCount,
            completed = Convert.ToBoolean(request.completed),
            date = DateTimeOffset.UtcNow
        };

        LeaderboardEntry? previousEntry =
            this._realm.All<LeaderboardEntry>().FirstOrDefault(e => e.levelId == levelId && e.user == user);

        this._realm.Write(() =>
        {
            // If there is a previous entry, and it's more than the new one, remove it and replace it with the new one
            if (previousEntry != null && previousEntry.score > entry.score)
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

    public (LeaderboardEntry[], int) GetLeaderboardEntries(string levelId, int from, int count)
    {
        IEnumerable<LeaderboardEntry> entries = this._realm.All<LeaderboardEntry>()
            .AsEnumerable()
            .Where(e=>e.completed)
            .Where(e => e.levelId == levelId);

        int totalEntries = entries.Count();

        IEnumerable<LeaderboardEntry> selectedEntries = entries
            .OrderBy(e => e.score)
            .Skip(from)
            .Take(count);

        return (selectedEntries.ToArray(), totalEntries);
    }

    public LeaderboardEntry? GetLeaderboardEntryFromPlayer(GameUser user, string levelId)
    {
        return this._realm.All<LeaderboardEntry>().Where(e=>e.completed).FirstOrDefault(e => e.user == user && e.levelId == levelId);
    }

    public int GetPositionOfLeaderboardEntry(LeaderboardEntry entry)
    {
        return this._realm.All<LeaderboardEntry>().Count(e => e.levelId == entry.levelId && e.score < entry.score) + 1;
    }
}