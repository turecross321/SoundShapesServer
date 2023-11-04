using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class EventFilters : IFilters
{
    [FilterProperty("actors", "Filter out events that were not done by any of the users with specified IDs.")]
    public GameUser[]? Actors { get; set; }

    [FilterProperty("onUser", "Filter out events that were not done on user with specified ID.")]
    public GameUser? OnUser { get; set; }

    [FilterProperty("onLevel", "Filter out events that were not done on level with specified ID.")]
    public GameLevel? OnLevel { get; set; }

    [FilterProperty("eventTypes", "Filter out events that do not match any of the specified types.")]
    public int[]? EventTypes { get; set; }

    [FilterProperty("createdBefore", "Filter out events that were not created before specified date.")]
    public DateTimeOffset? CreatedBefore { get; init; }

    [FilterProperty("createdAfter", "Filter out events that were not created after specified date.")]
    public DateTimeOffset? CreatedAfter { get; init; }
}