using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    [DocPropertyQuery("byUser", "Filter out entries that were not achieved by user with specified ID.")] 
    public GameUser? ByUser { get; init; }
    [DocPropertyQuery("completed", "Filter out entries based on whether they beat the level or not.")]
    public bool? Completed { get; init; }
    [DocPropertyQuery("obsolete", "Filter out entries based on whether they are obsolete or not.")]
    public bool? Obsolete { get; init; }
}