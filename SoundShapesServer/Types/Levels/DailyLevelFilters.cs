namespace SoundShapesServer.Types.Levels;

public class DailyLevelFilters
{
    public DailyLevelFilters(DateTimeOffset? date = null, bool? lastDate = null)
    {
        Date = date?.Date;
        LastDate = lastDate;
    }
    public DateTimeOffset? Date { get; set; }
    public bool? LastDate { get; set; }
}