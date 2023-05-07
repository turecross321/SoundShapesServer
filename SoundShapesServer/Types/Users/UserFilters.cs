namespace SoundShapesServer.Types.Users;

public class UserFilters
{
    public UserFilters(GameUser? isFollowing = null, GameUser? followedBy = null, string? search = null)
    {
        IsFollowing = isFollowing;
        FollowedBy = followedBy;
        Search = search;
    }

    public GameUser? IsFollowing;
    public GameUser? FollowedBy;
    public string? Search;
}