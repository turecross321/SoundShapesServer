using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public IQueryable<DailyLevel> GetDailyLevelObjects(DateTimeOffset? date = null, bool getOldLevelsIfThereAreNone = false)
    {
        List<DailyLevel> dailyLevels = _realm.All<DailyLevel>()
            .OrderByDescending(l => l.Date)
            .ToList();

        if (date == null)
        {
            return getOldLevelsIfThereAreNone ? dailyLevels.AsQueryable() : Enumerable.Empty<DailyLevel>().AsQueryable();
        }
        
        IQueryable<DailyLevel> levelsToday = dailyLevels.Where(d => d.Date.Date == date.Value.Date).AsQueryable();
        return levelsToday.Any()
            ? levelsToday
            : getOldLevelsIfThereAreNone ? dailyLevels.AsQueryable() : Enumerable.Empty<DailyLevel>().AsQueryable();
    }

    public DailyLevel? GetDailyLevelWithId(string id)
    {
        return _realm.All<DailyLevel>().FirstOrDefault(d => d.Id == id);
    }
    public DailyLevel AddDailyLevel(GameLevel level, DateTimeOffset date)
    {
        DailyLevel dailyLevel = new()
        {
            Id = GenerateGuid(), 
            Level = level, 
            Date = date
        };

        _realm.Write(() =>
        {
            _realm.Add(dailyLevel);
        });

        return dailyLevel;
    }

    public void RemoveDailyLevel(DailyLevel dailyLevel)
    {
        _realm.Write(() =>
        {
            _realm.Remove(dailyLevel);
        });
    }
}