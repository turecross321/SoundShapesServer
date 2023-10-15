using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static partial class UserHelper
{
    [System.Text.RegularExpressions.GeneratedRegex("^[A-Za-z][A-Za-z0-9-_]{2,15}$")]
    private static partial System.Text.RegularExpressions.Regex UsernameRegex();
    public static bool IsUsernameLegal(string username)
    {
        return UsernameRegex().IsMatch(username);
    }
    
    public static UserOrderType GetUserOrderType(this RequestContext context)
    {
        string? orderString = context.QueryString["orderBy"];
        
        return orderString switch
        {
            "followers" => UserOrderType.Followers,
            "following" => UserOrderType.Following,
            "publishedLevels" => UserOrderType.PublishedLevels,
            "likedLevels" => UserOrderType.LikedLevels,
            "creationDate" => UserOrderType.CreationDate,
            "playedLevels" => UserOrderType.PlayedLevels,
            "completedLevels" => UserOrderType.CompletedLevels,
            "totalDeaths" => UserOrderType.Deaths,
            "totalPlayTime" => UserOrderType.TotalPlayTime,
            "lastGameLogin" => UserOrderType.LastGameLogin,
            "events" => UserOrderType.Events,
            _ => UserOrderType.CreationDate
        };
    }

    public static UserFilters GetUserFilters(RequestContext context, GameDatabaseContext database)
    {
        string? isFollowingId = context.QueryString["isFollowing"]; 
        GameUser? isFollowing = null;
        if (isFollowingId != null) isFollowing = database.GetUserWithId(isFollowingId);
        
        string? followedById = context.QueryString["followedBy"];
        GameUser? followedBy = null;
        if (followedById != null) followedBy = database.GetUserWithId(followedById);
        
        string? searchQuery = context.QueryString["search"];

        return new UserFilters(isFollowing, followedBy, searchQuery);
    }
}