using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Extensions.Queryable;

public static class LeaderboardQueryableExtensions
{
    public static IQueryable<LeaderboardEntry> FilterLeaderboard(this IQueryable<LeaderboardEntry> entries,
        LeaderboardFilters filters)
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

        if (filters.Obsolete != null)
        {
            List<LeaderboardEntry> newEntries = new();
            if (filters.Obsolete == true)
                newEntries = entries.ToList();
            
            
            List<string> previousUserIds = new();
            // The lower "Score", the higher the score actually is because scores don't start from 0, and they decrease.
            foreach (LeaderboardEntry entry in entries.OrderBy(e => e.Score))
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
        return order switch
        {
            LeaderboardOrderType.Score => entries.OrderByDynamic(e => e.Score, descending),
            LeaderboardOrderType.PlayTime => entries.OrderByDynamic(e => e.PlayTime, descending),
            LeaderboardOrderType.Notes => entries.OrderByDynamic(e => e.Notes, descending),
            LeaderboardOrderType.CreationDate => entries.OrderByDynamic(e => e.CreationDate, descending),
            _ => entries.OrderLeaderboard(LeaderboardOrderType.Score, descending)
        };
    }

}