using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class EventFilters
{
    [DocPropertyQuery("actors", "Filter out events that were not done by any of the users with specified IDs.")]
    public GameUser[]? Actors { get; set; }
    [DocPropertyQuery("onUser", "Filter out events that were not done on user with specified ID.")]
    public GameUser? OnUser { get; set; }
    [DocPropertyQuery("onLevel", "Filter out events that were not done on level with specified ID.")]
    public GameLevel? OnLevel { get; set; }
    [DocPropertyQuery("eventTypes", "Filter out events that do not match any of the specified types.")]
    public int[]? EventTypes { get; set; }
    
    [DocPropertyQuery("createdBefore", "Filter out events that were not created before specified date.")]
    public DateTimeOffset? CreatedBefore { get; init; }
    [DocPropertyQuery("createdAfter", "Filter out events that were not created after specified date.")]
    public DateTimeOffset? CreatedAfter { get; init; }
}