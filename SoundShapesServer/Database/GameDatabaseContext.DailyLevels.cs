using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DailyLevel CreateDailyLevel(GameUser user, GameLevel level, DateTimeOffset date)
    {
        DailyLevel dailyLevel = new()
        {
            Id = GenerateGuid(), 
            Level = level, 
            Date = date.Date,
            Author = user
        };

        _realm.Write(() =>
        {
            _realm.Add(dailyLevel);
        });

        return dailyLevel;
    }

    public DailyLevel EditDailyLevel(DailyLevel daily, GameLevel level, DateTimeOffset date)
    {
        _realm.Write(() =>
        {
            daily.Date = date;
            daily.Level = level;
        });

        return daily;
    }
    public void RemoveDailyLevel(DailyLevel dailyLevel)
    {
        _realm.Write(() =>
        {
            _realm.Remove(dailyLevel);
        });
    }
    
    public DailyLevel? GetDailyLevelWithId(string id)
    {
        return _realm.All<DailyLevel>().FirstOrDefault(d => d.Id == id);
    }
    
    public IQueryable<DailyLevel> GetDailyLevels(DailyLevelOrderType order, bool descending, DailyLevelFilters filters)
    {
        IQueryable<DailyLevel> dailyLevels = _realm.All<DailyLevel>();

        IQueryable<DailyLevel> filteredDailyLevels = FilterDailyLevels(dailyLevels, filters);
        IQueryable<DailyLevel> orderedDailyLevels = OrderDailyLevels(filteredDailyLevels, order, descending);
        
        return orderedDailyLevels;
    }

    private static IQueryable<DailyLevel> FilterDailyLevels(IQueryable<DailyLevel> dailyLevels, DailyLevelFilters filters)
    {
        IQueryable<DailyLevel> response = dailyLevels;

        if (filters.LastDate == true)
        {
            response = response
                .OrderByDescending(d => d.Date);

            IList<DailyLevel> dailiesOnLastDate = new List<DailyLevel>();

            if (response.Any())
            {
                DateTimeOffset lastDate = response.ToArray()[0].Date.Date;

                foreach (DailyLevel dailyLevel in response)
                {
                    if (dailyLevel.Date < lastDate) break;
                    
                    dailiesOnLastDate.Add(dailyLevel);
                }
            }

            response = dailiesOnLastDate.AsQueryable();
        }

        if (filters.Date != null)
        {
            response = response
                .AsEnumerable()
                .Where(d => d.Date.Date == filters.Date?.Date)
                .AsQueryable();
        }

        return response;
    }
    

    #region Daily Level Ordering

    private IQueryable<DailyLevel> OrderDailyLevels(IQueryable<DailyLevel> dailyLevels, DailyLevelOrderType order,
        bool descending)
    {
        return order switch
        {
            DailyLevelOrderType.Date => OrderDailyLevelsByDate(dailyLevels, descending),
            _ => OrderDailyLevelsByDate(dailyLevels, descending)
        };
    }
    
    private static IQueryable<DailyLevel> OrderDailyLevelsByDate(IQueryable<DailyLevel> dailyLevels, bool descending)
    {
        if (descending) return dailyLevels.OrderByDescending(d => d.Date);
        return dailyLevels.OrderBy(d => d.Date);
    }

    #endregion
}