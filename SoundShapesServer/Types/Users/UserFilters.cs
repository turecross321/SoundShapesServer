namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    public UserFilters(GameUser? isFollowingUser = null, GameUser? followedByUser = null, string? search = null)
    {
        IsFollowingUser = isFollowingUser;
        FollowedByUser = followedByUser;
        Search = search;
    }

    public GameUser? IsFollowingUser;
    public GameUser? FollowedByUser;
    public string? Search;
}