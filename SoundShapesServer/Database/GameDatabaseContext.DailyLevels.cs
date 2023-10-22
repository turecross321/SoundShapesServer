using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
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
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow,
            Author = user
        };

        _realm.Write(() =>
        {
            _realm.Add(dailyLevel);
        });
        
        CreateEvent(user, EventType.DailyCreation, PlatformType.Unknown, EventDataType.Level, level.Id);

        return dailyLevel;
    }

    public DailyLevel EditDailyLevel(DailyLevel daily, GameUser user, GameLevel level, DateTimeOffset date)
    {
        _realm.Write(() =>
        {
            daily.Author = user;
            daily.Date = date;
            daily.Level = level;
            daily.ModificationDate = DateTimeOffset.UtcNow;
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
    
    public (DailyLevel[], int) GetPaginatedDailyLevels(DailyLevelOrderType order, bool descending, DailyLevelFilters filters, int from, int count)
    {
        return GetDailyLevels(order, descending, filters).Paginate(from, count);
    }

    public IQueryable<DailyLevel> GetDailyLevels(DailyLevelOrderType order, bool descending, DailyLevelFilters filters)
    {
        return _realm.All<DailyLevel>().FilterDailyLevels(filters).OrderDailyLevels(order, descending);
    }
}