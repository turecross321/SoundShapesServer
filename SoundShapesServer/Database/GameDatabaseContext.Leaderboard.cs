using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public void CreateLeaderboardEntry(LeaderboardSubmissionRequest request, GameUser user, string levelId)
    {
        LeaderboardEntry entry = new (GenerateGuid(), user, levelId, request);

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });
        
        CreateEvent(user, EventType.ScoreSubmission, null, null, entry);
        
        GameLevel? level = GetLevelWithId(levelId);
        if (level != null) SetLevelPlayTime(level);
        
        SetUserPlayTime(user);
    }
    
    public (IQueryable<LeaderboardEntry>, LeaderboardEntry[]) GetLeaderboardEntries(LeaderboardOrderType order, bool descending, LeaderboardFilters filters, int from, int count)
    {
        IQueryable<LeaderboardEntry> orderedEntries = order switch
        {
            LeaderboardOrderType.Score => LeaderboardOrderedByScore(descending),
            LeaderboardOrderType.PlayTime => LeaderboardOrderedByPlayTime(descending),
            LeaderboardOrderType.TokenCount => LeaderboardOrderedByTokenCount(descending),
            LeaderboardOrderType.Date => LeaderboardOrderedByDate(descending),
            _ => LeaderboardOrderedByScore(descending),
        };

        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(orderedEntries, filters);
        LeaderboardEntry[] paginatedEntries = PaginateLeaderboardEntries(filteredEntries, from, count);

        return (filteredEntries, paginatedEntries);
    }

    private IQueryable<LeaderboardEntry> LeaderboardOrderedByScore(bool descending)
    {
        if (descending) return _realm.All<LeaderboardEntry>().OrderByDescending(e => e.Score);
        return _realm.All<LeaderboardEntry>().OrderBy(e => e.Score);
    }

    private IQueryable<LeaderboardEntry> LeaderboardOrderedByPlayTime(bool descending)
    {
        if (descending) return _realm.All<LeaderboardEntry>().OrderByDescending(e => e.PlayTime);
        return _realm.All<LeaderboardEntry>().OrderBy(e => e.PlayTime);
    }
    
    private IQueryable<LeaderboardEntry> LeaderboardOrderedByTokenCount(bool descending)
    {
        if (descending) return _realm.All<LeaderboardEntry>().OrderByDescending(e => e.Tokens);
        return _realm.All<LeaderboardEntry>().OrderBy(e => e.Tokens);
    }
    
    private IQueryable<LeaderboardEntry> LeaderboardOrderedByDate(bool descending)
    {
        if (descending) return _realm.All<LeaderboardEntry>().OrderByDescending(e => e.Date);
        return _realm.All<LeaderboardEntry>().OrderBy(e => e.Date);
    }

    private static IQueryable<LeaderboardEntry> FilterLeaderboard(IQueryable<LeaderboardEntry> entries, LeaderboardFilters filters)
    {
        IQueryable<LeaderboardEntry> response = entries;
        
        if (filters.OnLevel != null)
        {
            response = response.Where(e => e.LevelId == filters.OnLevel);
        }

        if (filters.ByUser != null)
        {
            response = response.Where(e => e.User == filters.ByUser);
        }

        if (filters.Completed != null)
        {
            response = response.Where(e => e.Completed == filters.Completed);
        }
        
        if (filters.OnlyBest)
        {
            List<LeaderboardEntry> bestEntries = new ();

            List<string> previousUserIds = new ();
            // The lower "Score", the higher the score actually is because scores don't start from 0, and they decrease.
            foreach (LeaderboardEntry entry in response.OrderBy(e=>e.Score))
            {
                if (previousUserIds.Contains(entry.User.Id)) continue;
                
                bestEntries.Add(entry);
                previousUserIds.Add(entry.User);
            }

            response = bestEntries.AsQueryable();
        }
        
        return response;
    }

    public LeaderboardEntry? GetLeaderboardEntryWithId(string id)
    {
        return _realm.All<LeaderboardEntry>().FirstOrDefault(e => e.Id == id);
    }
    
    public int GetEntryPlacement(LeaderboardEntry entry)
    {
        IQueryable<LeaderboardEntry> entries = LeaderboardOrderedByScore(false);
        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(entries,
            new LeaderboardFilters(onLevel: entry.LevelId, completed: true, onlyBest: true));

        return filteredEntries.ToList().IndexOf(entry);
    }
    
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        RemoveAllReportsWithContentLeaderboardEntry(entry);
        
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}