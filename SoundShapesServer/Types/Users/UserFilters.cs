namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    public UserFilters(GameUser? isFollowingUser = null, GameUser? followedByUser = null, string? search = null)
    {
        IsFollowingUser = isFollowingUser;
        FollowedByUser = followedByUser;
        Search = search;
    }

    public readonly GameUser? IsFollowingUser;
    public readonly GameUser? FollowedByUser;
    public readonly string? Search;
}