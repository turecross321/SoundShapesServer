using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Extensions.Queryable;

public static class DailyLevelExtensions
{
    public static IQueryable<DailyLevel> FilterDailyLevels(this IQueryable<DailyLevel> dailyLevels, DailyLevelFilters filters)
    {
        if (filters.LatestDate == true)
        {
            IQueryable<DailyLevel> temp = dailyLevels
                .OrderByDescending(d => d.Date);
            
            if (temp.Any())
            {
                filters.Date = temp.First().Date;
            }
        }

        if (filters.Date != null)
        {
            DateTimeOffset date = filters.Date.Value.Date;
            dailyLevels = dailyLevels.AsEnumerable().Where(d => d.Date.Date == date).AsQueryable();
        }

        return dailyLevels;
    }
    
    public static IQueryable<DailyLevel> OrderDailyLevels(this IQueryable<DailyLevel> dailyLevels, DailyLevelOrderType order,
        bool descending)
    {
        return order switch
        {
            DailyLevelOrderType.Date => dailyLevels.OrderByDynamic(d => d.Date, descending),
            _ => dailyLevels.OrderDailyLevels(DailyLevelOrderType.Date, descending)
        };
    }
}