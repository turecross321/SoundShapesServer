using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public IQueryable<DailyLevel> GetDailyLevelObjects(DailyLevelOrderType order, bool descending, DailyLevelFilters filters)
    {
        IQueryable<DailyLevel> orderedDailyLevels = order switch
        {
            DailyLevelOrderType.Date => DailyLevelObjectsOrderedByDate(descending),
            _ => DailyLevelObjectsOrderedByDate(descending)
        };
        
        IQueryable<DailyLevel> filteredDailyLevels = FilterDailyLevels(orderedDailyLevels, filters);
        return filteredDailyLevels;
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
    
    private IQueryable<DailyLevel> DailyLevelObjectsOrderedByDate(bool descending)
    {
        if (descending) return _realm.All<DailyLevel>().OrderByDescending(d => d.Date);
        return _realm.All<DailyLevel>().OrderBy(d => d.Date);
    }

    public DailyLevel? GetDailyLevelWithId(string id)
    {
        return _realm.All<DailyLevel>().FirstOrDefault(d => d.Id == id);
    }
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
}