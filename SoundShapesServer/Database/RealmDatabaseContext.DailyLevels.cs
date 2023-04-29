using MongoDB.Bson;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<DailyLevel> DailyLevelObjects(DateTimeOffset? date = null)
    {
        List<DailyLevel> dailyLevels = this._realm.All<DailyLevel>()
            .OrderByDescending(l => l.Date)
            .ToList();

        if (date != null)
        {
            IQueryable<DailyLevel> levelsToday = dailyLevels.Where(d => d.Date.Date == date.Value.Date).AsQueryable();
            if (levelsToday.Any()) return levelsToday;
        }

        return dailyLevels.AsQueryable();
    }

    public DailyLevel? DailyLevelWithId(string id)
    {
        return this._realm.All<DailyLevel>().FirstOrDefault(d => d.Id == id);
    }
    public void AddDailyLevel(GameLevel level, DateTimeOffset date)
    {
        DailyLevel dailyLevel = new()
        {
            Id = GenerateGuid(),
            Level = level,
            Date = date
        };

        this._realm.Write(() =>
        {
            this._realm.Add(dailyLevel);
        });
    }

    public void RemoveDailyLevel(DailyLevel dailyLevel)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(dailyLevel);
        });
    }
}