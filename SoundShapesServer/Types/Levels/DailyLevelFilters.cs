using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Levels;

public class DailyLevelFilters : IFilters
{
    [FilterProperty("date", "Filter out daily levels that were not picked on specified date.")]
    public DateTimeOffset? Date { get; set; }

    [FilterProperty("latestDate",
        "Filter out daily levels that were not picked on the latest date that a level was picked on.")]
    public bool? LatestDate { get; init; }
}