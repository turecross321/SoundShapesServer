using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class DailyLevelHelper
{
    public static IQueryable<DailyLevel> FilterDailyLevels(IQueryable<DailyLevel> dailyLevels, string? date)
    {
        IQueryable<DailyLevel> response = dailyLevels;
        
        if (date != null)
        {
            DateTimeOffset dateObject = DateTimeOffset.Parse(date);
            response = response.AsEnumerable().Where(d => d.Date.Date == dateObject.Date).AsQueryable();
        }

        return response;
    }
}