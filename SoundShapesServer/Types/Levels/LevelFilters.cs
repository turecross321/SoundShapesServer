using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters : IFilters
{
    [FilterProperty("byUser", "Filter out levels that were not published by the user with specified ID.")]
    public GameUser? ByUser { get; init; }

    [FilterProperty("likedBy", "Filter out levels that are not liked by the user with specified ID.")]
    public GameUser? LikedByUser { get; init; }

    [FilterProperty("queuedBy", "Filter out levels that are not queued by the user with specified ID.")]
    public GameUser? QueuedByUser { get; init; }

    [FilterProperty("likedOrQueuedBy", "Filter out levels that are not liked or queued by the user with specified ID.")]
    public GameUser? LikedOrQueuedByUser { get; init; }

    [FilterProperty("completedBy", "Filter out levels that user with specified ID has not completed.")]
    public GameUser? CompletedBy { get; init; }

    [FilterProperty("inAlbum", "Filter out levels that are not in the album with specified ID.")]
    public GameAlbum? InAlbum { get; init; }

    [FilterProperty("inDaily", "Filter out levels that have never been picked as a daily level.")]
    public bool? InDaily { get; set; }

    [FilterProperty("inDailyDate", "Filter out levels that were not picked as a daily level on specified date.")]
    public DateTimeOffset? InDailyDate { get; set; }

    [FilterProperty("inLatestDaily",
        "Filter out levels that were not picked as a daily level on the date that the latest daily level was picked.")]
    public bool? InLatestDaily { get; set; }

    [FilterProperty("search",
        "Filter out levels whose name isn't included in query. Also includes levels where the author's username matches the query.")]
    public string? Search { get; init; }

    [FilterProperty("bpm", "Filter out levels that don't have the specified BPM.")]
    public int? Bpm { get; init; }

    [FilterProperty("scaleIndex", "Filter out levels that don't have the specified scale index.")]
    public int? ScaleIndex { get; init; }

    [FilterProperty("transposeValue", "Filter out levels that don't have the specified transpose value.")]
    public int? TransposeValue { get; init; }

    [FilterProperty("hasCar", "Filter out levels based on the presence or absence of cars.")]
    public bool? HasCar { get; init; }

    [FilterProperty("hasExplodingCar", "Filter out levels based on the presence or absence of exploding cars.")]
    public bool? HasExplodingCar { get; init; }

    [FilterProperty("hasUfo", "Filter out levels based on the presence or absence of UFOs.")]
    public bool? HasUfo { get; init; }

    [FilterProperty("hasFirefly", "Filter out levels based on the presence or absence of fireflies.")]
    public bool? HasFirefly { get; init; }

    [FilterProperty("uploadPlatforms",
        "Filter out levels that were not created on any of the specified upload platforms.")]
    public int[]? UploadPlatforms { get; init; }

    [FilterProperty("createdBefore", "Filter out levels that were not created before the specified date.")]
    public DateTimeOffset? CreatedBefore { get; init; }

    [FilterProperty("createdAfter", "Filter out levels that were not created after the specified date.")]
    public DateTimeOffset? CreatedAfter { get; init; }

    [FilterProperty("anyCompletions", "Filter out levels based on the presence or absence of any completions.")]
    public bool? AnyCompletions { get; init; }
}