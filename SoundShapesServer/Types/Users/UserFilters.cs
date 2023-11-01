using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    [FilterProperty("isFollowing", "Filter out users who are not following user with specified ID.")]
    public GameUser? IsFollowingUser { get; init; }
    [FilterProperty("followedBy", "Filter out users who are not followed by user with specified ID.")]
    public GameUser? FollowedByUser { get; init; }
    [FilterProperty("search", "Filter out users whose name isn't included in query.")]
    public string? Search { get; init; }
    [FilterProperty("createdBefore", "Filter out users who didn't create their account before specified date.")]
    public DateTimeOffset? CreatedBefore { get; init; }
    [FilterProperty("createdAfter", "Filter out users who didn't create their account after specified date.")]
    public DateTimeOffset? CreatedAfter { get; init; }

    public bool Deleted { get; set; } = false;
}