using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Extensions.Queryable;

public static class LeaderboardQueryableExtensions
{
    public static IQueryable<LeaderboardEntry> FilterLeaderboard(this IQueryable<LeaderboardEntry> entries, GameLevel level,
        LeaderboardFilters filters)
    {
        entries = entries.Where(e => e.Level == level);

        if (filters.ByUser != null)
        {
            entries = entries.Where(e => e.User == filters.ByUser);
        }
        
        if (filters.Completed != null)
        {
            entries = entries.Where(e => e.Completed == filters.Completed);
        }
        
        if (filters.Obsolete != null)
        {
            List<LeaderboardEntry> newEntries = new();
            if (filters.Obsolete == true)
                newEntries = entries.ToList();
            
            
            List<string> previousUserIds = new();
            // The lower "Score", the higher the score actually is because scores don't start from 0, and they decrease.
            foreach (LeaderboardEntry entry in entries.OrderByDescending(e => e.Completed).ThenBy(e => e.Score))
            {
                if (previousUserIds.Contains(entry.User.Id)) 
                    continue;

                if (filters.Obsolete == true)
                    newEntries.Remove(entry);
                else if (filters.Obsolete == false)
                {
                    newEntries.Add(entry);
                }
                
                previousUserIds.Add(entry.User.Id);
            }

            entries = newEntries.AsQueryable();
        }

        return entries;
    }
    
    public static IQueryable<LeaderboardEntry> OrderLeaderboard(this IQueryable<LeaderboardEntry> entries,
        LeaderboardOrderType order, bool descending)
    {
        IOrderedQueryable<LeaderboardEntry> ordered = order switch
        {
            LeaderboardOrderType.Score => entries.OrderByDynamic(e => e.Score, descending),
            LeaderboardOrderType.PlayTime => entries.OrderByDynamic(e => e.PlayTime, descending),
            LeaderboardOrderType.Notes => entries.OrderByDynamic(e => e.Notes, descending),
            LeaderboardOrderType.CreationDate => entries.OrderByDynamic(e => e.CreationDate, descending),
            _ => throw new ArgumentOutOfRangeException(nameof(order), order, "Invalid leaderboard order type")
        };
        
        return ordered.ThenByDynamic(e => e.CreationDate, descending);
    }

}