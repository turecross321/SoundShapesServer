using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Reports;

public class ReportFilters
{
    [FilterProperty("contentType", "Filter reports that are not associated with content of specified type.")]
    public int? ContentType { get; init; }
    [FilterProperty("onUser", "Filter reports that are not on specified user.")]
    public GameUser? ContentUser { get; init; }
    [FilterProperty("onLevel", "Filter reports that are not on specified level.")]
    public GameLevel? ContentLevel { get; init; }
    [FilterProperty("onLeaderboardEntry", "Filter reports that are not on specified leaderboard entry.")]
    public LeaderboardEntry? ContentLeaderboardEntry { get; init; }
    [FilterProperty("reasonType", "Filter reports that are not of certain type.")]
    public int? ReasonType { get; init; }
}