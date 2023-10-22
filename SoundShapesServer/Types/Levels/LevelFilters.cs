using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    public GameUser? ByUser { get; init; }
    public GameUser? LikedByUser { get; init; }
    public GameUser? QueuedByUser { get; init; }
    public GameUser? LikedOrQueuedByUser { get; init; }
    public GameUser? CompletedBy { get; init; }
    public GameAlbum? InAlbum { get; init; }
    public bool? InDaily { get; set; }
    public DateTimeOffset? InDailyDate { get; set; }
    public bool? InLatestDaily { get; set; }
    public string? Search { get; init; }
    public int? Bpm { get; init; }
    public int? ScaleIndex { get; init; }
    public int? TransposeValue { get; init; }
    public bool? HasCar { get; init; }
    public bool? HasExplodingCar { get; init; }
    public bool? HasUfo { get; init; }
    public bool? HasFirefly { get; init; }
    public List<PlatformType>? UploadPlatforms { get; init; }
    public DateTimeOffset? CreatedBefore { get; init; }
    public DateTimeOffset? CreatedAfter { get; init; }
    public bool? AnyCompletions { get; init; }
}