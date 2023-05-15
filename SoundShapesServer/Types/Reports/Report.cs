using Realms;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Reports;

public class Report : RealmObject
{
    [PrimaryKey] [Required] public string Id { get; init; } = "";
    public int ContentType { get; init; }
    public GameUser? ContentUser { get; set; }
    public GameLevel? ContentLevel { get; set; }
    public LeaderboardEntry? ContentLeaderboardEntry { get; set; }
    public int ReasonType { get; init; }
    public DateTimeOffset Date { get; init; }
    public GameUser Issuer { get; init; } = new();
}