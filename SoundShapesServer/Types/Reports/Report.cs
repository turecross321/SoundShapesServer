using Realms;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Reports;

public class Report : RealmObject
{
    [PrimaryKey] [Required] public string Id { get; init; } = "";
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with ContentType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _ContentType { get; set; }
    public ReportContentType ContentType
    {
        get => (ReportContentType)_ContentType;
        set => _ContentType = (int)value;
    }
    public GameUser? ContentUser { get; set; }
    public GameLevel? ContentLevel { get; set; }
    public LeaderboardEntry? ContentLeaderboardEntry { get; set; }
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with ReasonType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _ReasonType { get; set; }
    public ReportReasonType ReasonType
    {
        get => (ReportReasonType)_ReasonType;
        set => _ReasonType = (int)value;
    }
    public DateTimeOffset CreationDate { get; set; }
    public GameUser Author { get; set; }
}