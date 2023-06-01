using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public LeaderboardEntry CreateLeaderboardEntry(LeaderboardSubmissionRequest request, GameUser user, string levelId)
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
        return entry;
    }

    public int GetLeaderboardEntryPosition(LeaderboardEntry entry)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();
        
        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(entries,
            new LeaderboardFilters(onLevel: entry.LevelId, completed: true, onlyBest: true));
        IQueryable<LeaderboardEntry> orderedEntries = OrderLeaderboardByScore(filteredEntries, false);

        return orderedEntries.ToList().IndexOf(entry);
    }
    
    public void RemoveLeaderboardEntry(LeaderboardEntry entry)
    {
        RemoveAllReportsWithContentLeaderboardEntry(entry);
        
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
    
    public LeaderboardEntry? GetLeaderboardEntryWithId(string id)
    {
        return _realm.All<LeaderboardEntry>().FirstOrDefault(e => e.Id == id);
    }
    
    public (IQueryable<LeaderboardEntry>, LeaderboardEntry[]) GetLeaderboardEntries(LeaderboardOrderType order, bool descending, LeaderboardFilters filters, int from, int count)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();

        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(entries, filters);
        IQueryable<LeaderboardEntry> orderedEntries = OrderLeaderboard(filteredEntries, order, descending);
        
        LeaderboardEntry[] paginatedEntries = PaginateLeaderboardEntries(orderedEntries, from, count);

        return (filteredEntries, paginatedEntries);
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
            List<LeaderboardEntry> bestEntries = new();

            List<string> previousUserIds = new();
            // The lower "Score", the higher the score actually is because scores don't start from 0, and they decrease.
            foreach (LeaderboardEntry entry in response.OrderBy(e=>e.Score))
            {
                if (previousUserIds.Contains(entry.User.Id)) continue;
                
                bestEntries.Add(entry);
                previousUserIds.Add(entry.User.Id);
            }

            response = bestEntries.AsQueryable();
        }
        
        return response;
    }

    #region Leaderboard Ordering

    private static IQueryable<LeaderboardEntry> OrderLeaderboard(IQueryable<LeaderboardEntry> entries,
        LeaderboardOrderType order, bool descending)
    {
        return order switch
        {
            LeaderboardOrderType.Score => OrderLeaderboardByScore(entries, descending),
            LeaderboardOrderType.PlayTime => OrderLeaderboardByPlayTime(entries, descending),
            LeaderboardOrderType.TokenCount => OrderLeaderboardByTokenCount(entries, descending),
            LeaderboardOrderType.Date => OrderLeaderboardByDate(entries, descending),
            _ => OrderLeaderboardByScore(entries, descending),
        };
    }
    
    private static IQueryable<LeaderboardEntry> OrderLeaderboardByScore(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.Score);
        return entries.OrderBy(e => e.Score);
    }

    private static IQueryable<LeaderboardEntry> OrderLeaderboardByPlayTime(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.PlayTime);
        return entries.OrderBy(e => e.PlayTime);
    }
    
    private static IQueryable<LeaderboardEntry> OrderLeaderboardByTokenCount(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.Tokens);
        return entries.OrderBy(e => e.Tokens);
    }
    
    private static IQueryable<LeaderboardEntry> OrderLeaderboardByDate(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.Date);
        return entries.OrderBy(e => e.Date);
    }
    
    #endregion
}