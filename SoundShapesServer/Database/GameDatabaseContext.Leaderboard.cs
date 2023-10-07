using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public LeaderboardEntry CreateLeaderboardEntry(LeaderboardSubmissionRequest request, GameUser user, GameLevel level, PlatformType platformType)
    {
        LeaderboardEntry entry = new()
        {
            Id = GenerateGuid(),
            User = user,
            Level = level,
            Score = request.Score,
            PlayTime = Math.Max(request.PlayTime, 0),
            Deaths = Math.Max(request.Deaths, 0),
            Golded = request.Golded,
            Notes = request.Notes,
            Completed = request.Completed,
            CreationDate = DateTimeOffset.UtcNow,
            PlatformType = platformType
        };

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });
        
        SetLevelPlayTime(level);
        CreateEvent(user, EventType.ScoreSubmission, platformType, null, level, entry);
        SetUserPlayTime(user);
        
        return entry;
    }

    public int GetLeaderboardEntryPosition(LeaderboardEntry entry, GameUser? accessor)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();
        
        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(entries,
            new LeaderboardFilters(onLevel: entry.Level, completed: true, onlyBest: true), accessor);
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

    public (int, LeaderboardEntry[]) GetPaginatedLeaderboardEntries(LeaderboardOrderType order, bool descending, LeaderboardFilters filters, int from, int count, GameUser? accessor)
    {
        IQueryable<LeaderboardEntry> orderedEntries = GetLeaderboardEntries(order, descending, filters, accessor);
        LeaderboardEntry[] paginatedEntries = PaginateLeaderboardEntries(orderedEntries, from, count);

        return (orderedEntries.Count(), paginatedEntries);
    }
    
    public IQueryable<LeaderboardEntry> GetLeaderboardEntries(LeaderboardOrderType order, bool descending, LeaderboardFilters filters, GameUser? accessor)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();
        IQueryable<LeaderboardEntry> filteredEntries = FilterLeaderboard(entries, filters, accessor);
        IQueryable<LeaderboardEntry> orderedEntries = OrderLeaderboard(filteredEntries, order, descending);

        return orderedEntries;
    }

    private static IQueryable<LeaderboardEntry> FilterLeaderboard(IQueryable<LeaderboardEntry> entries,
        LeaderboardFilters filters, GameUser? accessor)
    {
        entries = entries.Where(e => e.Level == filters.OnLevel);

        if (filters.ByUser != null)
        {
            entries = entries.Where(e => e.User == filters.ByUser);
        }

        if (filters.Completed != null)
        {
            entries = entries.Where(e => e.Completed == filters.Completed);
        }

        if (filters.OnlyBest)
        {
            List<LeaderboardEntry> bestEntries = new();

            List<string> previousUserIds = new();
            // The lower "Score", the higher the score actually is because scores don't start from 0, and they decrease.
            foreach (LeaderboardEntry entry in entries.OrderBy(e => e.Score))
            {
                if (previousUserIds.Contains(entry.User.Id)) continue;

                bestEntries.Add(entry);
                previousUserIds.Add(entry.User.Id);
            }

            entries = bestEntries.AsQueryable();
        }

        // Automatically remove private and unlisted results
        if ((accessor?.PermissionsType ?? PermissionsType.Default) < PermissionsType.Moderator)
        {
            List<LeaderboardEntry> privateLevelEntries = new();

            foreach (LeaderboardEntry entry in entries)
            {
                if (entry.Level.Author.Id != accessor?.Id && entry.Level.Visibility == LevelVisibility.Private)
                {
                    privateLevelEntries.Add(entry);
                }

                entries = entries.AsEnumerable().Except(privateLevelEntries).AsQueryable();
            }
        }

        return entries;
        }

    #region Leaderboard Ordering

    private static IQueryable<LeaderboardEntry> OrderLeaderboard(IQueryable<LeaderboardEntry> entries,
        LeaderboardOrderType order, bool descending)
    {
        return order switch
        {
            LeaderboardOrderType.Score => OrderLeaderboardByScore(entries, descending),
            LeaderboardOrderType.PlayTime => OrderLeaderboardByPlayTime(entries, descending),
            LeaderboardOrderType.Notes => OrderLeaderboardByTokenCount(entries, descending),
            LeaderboardOrderType.CreationDate => OrderLeaderboardByDate(entries, descending),
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
        if (descending) return entries.OrderByDescending(e => e.Notes);
        return entries.OrderBy(e => e.Notes);
    }
    
    private static IQueryable<LeaderboardEntry> OrderLeaderboardByDate(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.CreationDate);
        return entries.OrderBy(e => e.CreationDate);
    }
    
    #endregion
}