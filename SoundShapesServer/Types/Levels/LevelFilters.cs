using AttribDoc.Attributes;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    [DocPropertyQuery("byUser", "Filter out levels that were not published by the user with specified ID.")] 
    public GameUser? ByUser { get; init; }
    [DocPropertyQuery("likedBy", "Filter out levels that are not liked by the user with specified ID.")] 
    public GameUser? LikedByUser { get; init; }
    [DocPropertyQuery("queuedBy", "Filter out levels that are not queued by the user with specified ID.")] 
    public GameUser? QueuedByUser { get; init; }
    [DocPropertyQuery("likedOrQueuedBy", "Filter out levels that are not liked or queued by the user with specified ID.")]
    public GameUser? LikedOrQueuedByUser { get; init; }
    [DocPropertyQuery("completedBy", "Filter out levels that user with specified ID has not completed.")]
    public GameUser? CompletedBy { get; init; }
    [DocPropertyQuery("completedBy", "Filter out levels that are not in the album with specified ID.")]
    public GameAlbum? InAlbum { get; init; }
    [DocPropertyQuery("inDaily", "Filter out levels that have never been picked as a daily level.")]
    public bool? InDaily { get; set; }
    [DocPropertyQuery("inDailyDate", "Filter out levels that were not picked as a daily level on specified date.")]
    public DateTimeOffset? InDailyDate { get; set; }
    [DocPropertyQuery("inLatestDaily", "Filter out levels that were not picked as a daily level on the date that the latest daily level was picked.")]
    public bool? InLatestDaily { get; set; }
    [DocPropertyQuery("search", "Filter out levels based on whether they were picked as a daily level on the latest date or not.")]
    public string? Search { get; init; }
    [DocPropertyQuery("bpm", "Filter out levels that don't have the specified BPM.")]
    public int? Bpm { get; init; }
    [DocPropertyQuery("scaleIndex", "Filter out levels that don't have the specified scale index.")]
    public int? ScaleIndex { get; init; }
    [DocPropertyQuery("transposeValue", "Filter out levels that don't have the specified transpose value.")]
    public int? TransposeValue { get; init; }
    [DocPropertyQuery("hasCar", "Filter out levels based on the presence or absence of cars.")]
    public bool? HasCar { get; init; }
    [DocPropertyQuery("hasExplodingCar", "Filter out levels based on the presence or absence of exploding cars.")]
    public bool? HasExplodingCar { get; init; }
    [DocPropertyQuery("hasUfo", "Filter out levels based on the presence or absence of UFOs.")]
    public bool? HasUfo { get; init; }
    [DocPropertyQuery("hasFirefly", "Filter out levels based on the presence or absence of fireflies.")]
    public bool? HasFirefly { get; init; }
    [DocPropertyQuery("uploadPlatforms", "Filter out levels that were not created on any of the specified upload platforms.")]
    public int[]? UploadPlatforms { get; init; }
    [DocPropertyQuery("createdBefore", "Filter out levels that were not created before the specified date.")]
    public DateTimeOffset? CreatedBefore { get; init; }
    [DocPropertyQuery("createdAfter", "Filter out levels that were not created after the specified date.")]
    public DateTimeOffset? CreatedAfter { get; init; }
    [DocPropertyQuery("anyCompletions", "Filter out levels based on the presence or absence of any completions.")]
    public bool? AnyCompletions { get; init; }
}