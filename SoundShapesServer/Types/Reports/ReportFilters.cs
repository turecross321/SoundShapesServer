using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Reports;

public class ReportFilters
{
    [DocPropertyQuery("contentType", "Filter reports that are not associated with content of specified type.")]
    public int? ContentType { get; init; }
    [DocPropertyQuery("onUser", "Filter reports that are not on specified user.")]
    public GameUser? ContentUser { get; init; }
    [DocPropertyQuery("onLevel", "Filter reports that are not on specified level.")]
    public GameLevel? ContentLevel { get; init; }
    [DocPropertyQuery("onLeaderboardEntry", "Filter reports that are not on specified leaderboard entry.")]
    public LeaderboardEntry? ContentLeaderboardEntry { get; init; }
    [DocPropertyQuery("reasonType", "Filter reports that are not of certain type.")]
    public int? ReasonType { get; init; }
}