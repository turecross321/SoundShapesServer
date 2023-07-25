namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    public UserFilters(GameUser? isFollowingUser = null, GameUser? followedByUser = null, string? search = null, bool? deleted = false)
    {
        IsFollowingUser = isFollowingUser;
        FollowedByUser = followedByUser;
        Search = search;
        Deleted = deleted;
    }

    public readonly GameUser? IsFollowingUser;
    public readonly GameUser? FollowedByUser;
    public readonly string? Search;
    public readonly bool? Deleted;
}