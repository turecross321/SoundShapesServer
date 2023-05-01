using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<DailyLevel> GetDailyLevelObjects(DateTimeOffset? date = null)
    {
        List<DailyLevel> dailyLevels = _realm.All<DailyLevel>()
            .OrderByDescending(l => l.Date)
            .ToList();

        if (date == null) return dailyLevels.AsQueryable();
        IQueryable<DailyLevel> levelsToday = dailyLevels.Where(d => d.Date.Date == date.Value.Date).AsQueryable();
        return levelsToday.Any() ? levelsToday : dailyLevels.AsQueryable();
    }

    public DailyLevel? GetDailyLevelWithId(string id)
    {
        return _realm.All<DailyLevel>().FirstOrDefault(d => d.Id == id);
    }
    public void AddDailyLevel(GameLevel level, DateTimeOffset date)
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
    }

    public void RemoveDailyLevel(DailyLevel dailyLevel)
    {
        _realm.Write(() =>
        {
            _realm.Remove(dailyLevel);
        });
    }
}