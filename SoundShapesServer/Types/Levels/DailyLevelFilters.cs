namespace SoundShapesServer.Types.Levels;

public class DailyLevelFilters
{
    public DailyLevelFilters(DateTimeOffset? date = null, bool? latestDate = null)
    {
        Date = date?.Date;
        LatestDate = latestDate;
    }
    public DateTimeOffset? Date { get; set; }
    public bool? LatestDate { get; set; }
}