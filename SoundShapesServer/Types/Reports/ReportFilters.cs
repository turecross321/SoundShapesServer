using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Reports;

public class ReportFilters
{
    public ReportContentType? ContentType { get; set; }
    public GameUser? ContentUser { get; set; }
    public GameLevel? ContentLevel { get; set; }
    public LeaderboardEntry? ContentLeaderboardEntry { get; set; }
    public ReportReasonType? ReasonType { get; set; }
}