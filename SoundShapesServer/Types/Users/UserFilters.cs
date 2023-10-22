namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    public GameUser? IsFollowingUser;
    public GameUser? FollowedByUser;
    public string? Search;
    public bool Deleted;
    public DateTimeOffset? CreatedBefore { get; init; }
    public DateTimeOffset? CreatedAfter { get; init; }
}