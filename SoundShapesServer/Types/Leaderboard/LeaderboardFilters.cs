using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters : IFilters
{
    [FilterProperty("byUser", "Filter out entries that were not achieved by user with specified ID.")]
    public GameUser? ByUser { get; set; }

    [FilterProperty("completed", "Filter out entries based on whether they beat the level or not.")]
    public bool? Completed { get; init; }

    [FilterProperty("obsolete", "Filter out entries based on whether they are obsolete or not.")]
    public bool? Obsolete { get; init; }
}